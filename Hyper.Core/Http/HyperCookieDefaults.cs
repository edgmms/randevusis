﻿namespace Hyper.Core.Http
{
    /// <summary>
    /// Represents default values related to cookies
    /// </summary>
    public static partial class HyperCookieDefaults
    {
        /// <summary>
        /// Gets the cookie name prefix
        /// </summary>
        public static string Prefix => ".Hyper";

        /// <summary>
        /// Gets a cookie name of the ApplicationUser
        /// </summary>
        public static string ApplicationUserCookie => ".ApplicationUser";

        /// <summary>
        /// Gets a cookie name of the antiforgery
        /// </summary>
        public static string AntiforgeryCookie => ".Antiforgery";

        /// <summary>
        /// Gets a cookie name of the session state
        /// </summary>
        public static string SessionCookie => ".Session";

        /// <summary>
        /// Gets a cookie name of the culture
        /// </summary>
        public static string CultureCookie => ".Culture";

        /// <summary>
        /// Gets a cookie name of the temp data
        /// </summary>
        public static string TempDataCookie => ".TempData";
 
        /// <summary>
        /// Gets a cookie name of the authentication
        /// </summary>
        public static string AuthenticationCookie => ".Authentication";

        /// <summary>
        /// Gets a cookie name of the external authentication
        /// </summary>
        public static string ExternalAuthenticationCookie => ".ExternalAuthentication";

        /// <summary>
        /// Gets a cookie name of the Eu Cookie Law Warning
        /// </summary>
        public static string IgnoreEuCookieLawWarning => ".IgnoreEuCookieLawWarning";
    }
}