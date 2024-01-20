using Hyper.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace Hyper.Web.Infrastructure.Routing
{
    public class HyperRedirectResultExecutor : RedirectResultExecutor
    {
        #region Fields

        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly SecuritySettings _securitySettings;

        #endregion

        #region Ctor

        public HyperRedirectResultExecutor(IActionContextAccessor actionContextAccessor,
            ILoggerFactory loggerFactory,
            IUrlHelperFactory urlHelperFactory,
            SecuritySettings securitySettings) : base(loggerFactory, urlHelperFactory)
        {
            _actionContextAccessor = actionContextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _securitySettings = securitySettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Execute passed redirect result
        /// </summary>
        /// <param name="context">Action context</param>
        /// <param name="result">Redirect result</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override Task ExecuteAsync(ActionContext context, RedirectResult result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            if (_securitySettings.AllowNonAsciiCharactersInHeaders)
            {
                //passed redirect URL may contain non-ASCII characters, that are not allowed now (see https://github.com/aspnet/KestrelHttpServer/issues/1144)
                //so we force to encode this URL before processing
                var url = WebUtility.UrlDecode(result.Url);
                var urlHelper = result.UrlHelper ?? _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
                var isLocalUrl = urlHelper.IsLocalUrl(url);
                var uri = new Uri(url, UriKind.Absolute);

                //Allowlist redirect URI schemes to http and https
                if ((uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) && urlHelper.IsLocalUrl(uri.AbsolutePath))
                    result.Url = isLocalUrl ? uri.PathAndQuery : $"{uri.GetLeftPart(UriPartial.Query)}{uri.Fragment}";
                else
                    result.Url = urlHelper.RouteUrl("Homepage");
            }

            return base.ExecuteAsync(context, result);
        }

        #endregion
    }
}
