using Microsoft.AspNetCore.Http;

namespace Hyper.Core.Http
{
    /// <summary>
    /// Represents default values related to authentication services
    /// </summary>
    public static partial class HyperAuthenticationDefaults
    {
        /// <summary>
        /// Get Application User Id Claim
        /// </summary>
        public static string ApplicationUserIdClaim => "applicationuserid";

        /// <summary>
        /// Get Application User Store Claim
        /// </summary>
        public static string ApplicationUserStoreClaim => "applicationuserstoreid";

        /// <summary>
        /// The default value used for authentication scheme
        /// </summary>
        public static string AuthenticationScheme => "Authentication";

        /// <summary>
        /// The default value used for external authentication scheme
        /// </summary>
        public static string ExternalAuthenticationScheme => "ExternalAuthentication";

        /// <summary>
        /// The issuer that should be used for any claims that are created
        /// </summary>
        public static string ClaimsIssuer => "hyper";

        /// <summary>
        /// The default value for the login path
        /// </summary>
        public static PathString LoginPath => new("/giris");

        /// <summary>
        /// The default value for the access denied path
        /// </summary>
        public static PathString AccessDeniedPath => new("/sayfa-bulunamadi");

        /// <summary>
        /// The default value of the return URL parameter
        /// </summary>
        public static string ReturnUrlParameter => string.Empty;
    }
}
