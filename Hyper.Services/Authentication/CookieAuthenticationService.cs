using Hyper.Core.Domain.ApplicationUsers;
using Hyper.Core.Http;
using Hyper.Services.ApplicationUsers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Hyper.Services.Authentication
{
    /// <summary>
    /// Represents service using cookie middleware for the authentication
    /// </summary>
    public partial class CookieAuthenticationService : IAuthenticationService
    {
        #region Fields

        private readonly IApplicationUserService _applicationUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private ApplicationUser _cachedApplicationUser;

        #endregion

        #region Ctor

        public CookieAuthenticationService(
            IApplicationUserService applicationUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            _applicationUserService = applicationUserService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="applicationUser">ApplicationUser</param>
        /// <param name="isPersistent">Whether the authentication session is persisted across multiple requests</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual void SignIn(ApplicationUser applicationUser, bool isPersistent)
        {
            if (applicationUser == null)
                throw new ArgumentNullException(nameof(applicationUser));

            //create claims for applicationUser's username and email
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(applicationUser.Username))
                claims.Add(new Claim(ClaimTypes.Name, applicationUser.Username, ClaimValueTypes.String, HyperAuthenticationDefaults.ClaimsIssuer));

            if (!string.IsNullOrEmpty(applicationUser.Email))
                claims.Add(new Claim(ClaimTypes.Email, applicationUser.Email, ClaimValueTypes.Email, HyperAuthenticationDefaults.ClaimsIssuer));

            claims.Add(new Claim(HyperAuthenticationDefaults.ApplicationUserIdClaim, applicationUser.Id.ToString(), ClaimValueTypes.Integer, HyperAuthenticationDefaults.ClaimsIssuer));
            claims.Add(new Claim(HyperAuthenticationDefaults.ApplicationUserStoreClaim, applicationUser.RegisteredInStoreId.ToString(), ClaimValueTypes.String, HyperAuthenticationDefaults.ClaimsIssuer));

            //create principal for the current authentication scheme
            var userIdentity = new ClaimsIdentity(claims, HyperAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            //set value indicating whether session is persisted and the time at which the authentication was issued
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.UtcNow
            };

            //sign in
            _httpContextAccessor.HttpContext.SignInAsync(HyperAuthenticationDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);

            //cache authenticated applicationUser
            _cachedApplicationUser = applicationUser;
        }

        /// <summary>
        /// Sign out
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual void SignOut()
        {
            //reset cached applicationUser
            _cachedApplicationUser = null;

            //and sign out from the current authentication scheme
            _httpContextAccessor.HttpContext.SignOutAsync(HyperAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Get authenticated applicationUser
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the applicationUser
        /// </returns>
        public virtual ApplicationUser GetAuthenticatedApplicationUser()
        {
            //whether there is a cached applicationUser
            if (_cachedApplicationUser != null)
                return _cachedApplicationUser;

            //try to get authenticated user identity
            var authenticateResult = _httpContextAccessor.HttpContext.AuthenticateAsync(HyperAuthenticationDefaults.AuthenticationScheme).Result;
            if (!authenticateResult.Succeeded)
                return null;

            ApplicationUser applicationUser = null;

            //try to get applicationUser by email
            var emailClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email
                && claim.Issuer.Equals(HyperAuthenticationDefaults.ClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
            if (emailClaim != null)
                applicationUser = _applicationUserService.GetApplicationUserByEmail(emailClaim.Value);

            //whether the found applicationUser is available
            if (applicationUser == null || !applicationUser.Active || applicationUser.RequireReLogin || applicationUser.Deleted)
                return null;

            //get the latest password
            var applicationUserPassword = _applicationUserService.GetCurrentPassword(applicationUser.Id);

            //required a applicationUser to re-login after password changing
            if (applicationUserPassword.CreatedOnUtc.CompareTo(authenticateResult.Properties.IssuedUtc.HasValue
                ? authenticateResult.Properties.IssuedUtc.Value.DateTime
                : DateTime.UtcNow) > 0)
                return null;

            //cache authenticated applicationUser
            _cachedApplicationUser = applicationUser;

            return _cachedApplicationUser;
        }

        #endregion
    }
}
