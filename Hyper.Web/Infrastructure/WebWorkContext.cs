
using Hyper.Core;
using Hyper.Core.Domain.ApplicationUsers;
using Hyper.Services.Authentication;

namespace Hyper.Web.Infrastructure
{
    public class WebWorkContext : IWorkContext
    {
        private ApplicationUser _cachedApplicationUser;
        private readonly IAuthenticationService _authenticationService;

        public WebWorkContext(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public ApplicationUser CurrentApplicationUser
        {
            get
            {
                if (_cachedApplicationUser is not null)
                    return _cachedApplicationUser;

                var applicationUser = _authenticationService.GetAuthenticatedApplicationUser();
                _cachedApplicationUser = applicationUser;
                return _cachedApplicationUser;
            }
        }

		public bool IsAdministrator
		{
			get { return this.CurrentApplicationUser.SystemName == ApplicationUserSystemNames.Administrator; }
		}

		public bool IsStoreAdministrator
		{
            get { return this.CurrentApplicationUser.SystemName == ApplicationUserSystemNames.StoreAdministrator; }
        }

		public bool IsStoreUser
		{
			get { return this.CurrentApplicationUser.SystemName == ApplicationUserSystemNames.StoreUser; }
		}
	}
}
