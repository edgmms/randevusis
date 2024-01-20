using Hyper.Services.Authentication;
using Hyper.Web.Infrastructure.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using System.Linq;
using System.Threading.Tasks;
using WebMarkupMin.AspNetCore7;

namespace Hyper.Web.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IApplicationBuilder
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Add exception handling
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseHyperExceptionHandler(this IApplicationBuilder application, bool isDevelopment)
        {
            if (isDevelopment)
            {
                //get detailed exceptions for developing and testing purposes
                application.UseDeveloperExceptionPage();
            }
            else
            {
                //or use special exception handler
                application.UseExceptionHandler("/Error/Error");
            }

            ////log errors
            //application.UseExceptionHandler(handler =>
            //{
            //    handler.Run(async context =>
            //    {
            //        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            //        if (exception == null)
            //            return;

            //        try
            //        {
            //            ////log error
            //            //await ILoggerFactory.ErrorAsync(exception.Message, exception);
            //        }
            //        finally
            //        {
            //            //rethrow the exception to show the error page
            //            ExceptionDispatchInfo.Throw(exception);
            //        }
            //    });
            //});
        }

        /// <summary>
        /// Adds a special handler that checks for responses with the 404 status code that do not have a body
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UsePageNotFound(this IApplicationBuilder application)
        {
            application.UseStatusCodePages(async context =>
            {
                await Task.Run(() =>
                {
                    //handle 404 Not Found
                    if (context.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                    {
                        //get original path and query
                        var originalPath = context.HttpContext.Request.Path;
                        var originalQueryString = context.HttpContext.Request.QueryString;

                        try
                        {
                            //get new path
                            var pageNotFoundPath = "/sayfa-bulunamadi";
                            //re-execute request with new path
                            context.HttpContext.Response.Redirect(context.HttpContext.Request.PathBase + pageNotFoundPath);
                        }
                        finally
                        {
                            //return original path to request
                            context.HttpContext.Request.QueryString = originalQueryString;
                            context.HttpContext.Request.Path = originalPath;
                        }
                    }
                });

            });
        }

        /// <summary>
        /// Adds a special handler that checks for responses with the 400 status code (bad request)
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseBadRequestResult(this IApplicationBuilder application)
        {
            application.UseStatusCodePages(async context =>
            {
                await Task.Run(() =>
                {
                    //handle 404 (Bad request)
                    if (context.HttpContext.Response.StatusCode == StatusCodes.Status400BadRequest)
                    {
                        //var logger = EngineContext.Current.Resolve<ILogger>();
                        //var workContext = EngineContext.Current.Resolve<IWorkContext>();
                        //await logger.ErrorAsync("Error 400. Bad request", null, customer: await workContext.GetCurrentCustomerAsync());
                    }
                });
            });
        }

        /// <summary>
        /// Configure middleware for dynamically compressing HTTP responses
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseHyperResponseCompression(this IApplicationBuilder application)
        {
            application.UseResponseCompression();
        }

        /// <summary>
        /// Adds WebOptimizer to the <see cref="IApplicationBuilder"/> request execution pipeline
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseHyperWebOptimizer(this IApplicationBuilder application)
        {
            application.UseWebOptimizer();
        }

        /// <summary>
        /// Configure static file serving
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseHyperStaticFiles(this IApplicationBuilder application)
        {
            void staticFileResponse(StaticFileResponseContext context)
            {
                context.Context.Response.Headers.Append(HeaderNames.CacheControl, "public,max-age=31536000");
            }

            //common static files
            application.UseStaticFiles(new StaticFileOptions { OnPrepareResponse = staticFileResponse });
        }


        /// <summary>
        /// Adds the authentication middleware, which enables authentication capabilities.
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseHyperAuthentication(this IApplicationBuilder application)
        {
            application.UseMiddleware<AuthenticationMiddleware>();
        }

        /// <summary>
        /// Configure Endpoints routing
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseHyperEndpoints(this IApplicationBuilder application)
        {
            //Execute the endpoint selected by the routing middleware
            application.UseEndpoints(endpoints =>
            {
                RouteProvider.RegisterRoutes(endpoints);
            });
        }

        /// <summary>
        /// Configure WebMarkupMin
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseHyperWebMarkupMin(this IApplicationBuilder application)
        {
            application.UseWebMarkupMin();
        }
    }
}
