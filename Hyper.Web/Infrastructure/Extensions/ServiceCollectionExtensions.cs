using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hyper.Core.Configuration;
using Hyper.Core.Http;
using Hyper.Core.Mapper;
using Hyper.Services.Authentication;
using Hyper.Web.Areas.Admin.Infrastructure.Mapper;
using Hyper.Web.Areas.Admin.Infrastructure.Mvc.ModelBinders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using StackExchange.Profiling.Storage;
using System;
using System.Linq;
using System.Net;
using WebMarkupMin.AspNetCore7;
using WebMarkupMin.Core;
using WebMarkupMin.NUglify;

namespace Hyper.Web.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure base application settings
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="builder">A builder for web applications and services</param>
        public static void ConfigureApplicationSettings(this IServiceCollection services,
            WebApplicationBuilder builder)
        {
            //let the operating system decide what TLS protocol version to use
            //see https://docs.microsoft.com/dotnet/framework/network-programming/tls
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

            //app settings
            HyperAppSettings hyperAppSettings = new();
            builder.Configuration.GetSection(nameof(HyperAppSettings)).Bind(hyperAppSettings);
            builder.Services.AddSingleton(hyperAppSettings);

            //application user settings
            ApplicationUserSettings applicationUserSettings = new();
            builder.Configuration.GetSection(nameof(ApplicationUserSettings)).Bind(applicationUserSettings);
            builder.Services.AddSingleton(applicationUserSettings);

            //security settings
            SecuritySettings securitySettings = new();
            builder.Configuration.GetSection(nameof(SecuritySettings)).Bind(securitySettings);
            builder.Services.AddSingleton(securitySettings);

            DefaultEmailAccountSettings defaultEmailAccountSettings = new();
            builder.Configuration.GetSection(nameof(DefaultEmailAccountSettings)).Bind(defaultEmailAccountSettings);
            builder.Services.AddSingleton(defaultEmailAccountSettings);
        }

        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void ConfigureHttpContextAccessor(this IServiceCollection services)
        {
            //add accessor to HttpContext
            services.AddHttpContextAccessor();
        }


        /// <summary>
        /// Register HttpContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// Configure AutoMapper Profiles
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void ConfigureAutoMapperProfiles(this IServiceCollection services)
        {
            //Automapper
            var config = new MapperConfiguration(cfg => { cfg.AddProfile(typeof(HyperProfile)); });
            AutoMapperConfiguration.Init(config);
        }

        /// <summary>
        /// Adds services required for anti-forgery support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAntiForgery(this IServiceCollection services)
        {
            //override cookie name
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = $"{HyperCookieDefaults.Prefix}{HyperCookieDefaults.AntiforgeryCookie}";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
        }

        /// <summary>
        /// Adds services required for application session state
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpSession(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = $"{HyperCookieDefaults.Prefix}{HyperCookieDefaults.SessionCookie}";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
        }

        ///// <summary>
        ///// Adds data protection services
        ///// </summary>
        ///// <param name="services">Collection of service descriptors</param>
        //public static void AddHyperDataProtection(this IServiceCollection services, string dataProtectionKeysPath)
        //{ 
        //    var dataProtectionKeysFolder = new System.IO.DirectoryInfo(dataProtectionKeysPath);
        //    //configure the data protection system to persist keys to the specified directory
        //    services.AddDataProtection().PersistKeysToFileSystem(dataProtectionKeysFolder);
        //}

        /// <summary>
        /// Adds authentication service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHyperAuthentication(this IServiceCollection services)
        {
            //set default authentication schemes
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = HyperAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = HyperAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = HyperAuthenticationDefaults.ExternalAuthenticationScheme;
            });

            //add main cookie authentication
            authenticationBuilder.AddCookie(HyperAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = $"{HyperCookieDefaults.Prefix}{HyperCookieDefaults.AuthenticationCookie}";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.LoginPath = HyperAuthenticationDefaults.LoginPath;
                options.AccessDeniedPath = HyperAuthenticationDefaults.AccessDeniedPath;
            });

            //add external authentication
            authenticationBuilder.AddCookie(HyperAuthenticationDefaults.ExternalAuthenticationScheme, options =>
            {
                options.Cookie.Name = $"{HyperCookieDefaults.Prefix}{HyperCookieDefaults.ExternalAuthenticationCookie}";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.LoginPath = HyperAuthenticationDefaults.LoginPath;
                options.AccessDeniedPath = HyperAuthenticationDefaults.AccessDeniedPath;
            });
        }

        /// <summary>
        /// Add and configure MVC for the application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <returns>A builder for configuring MVC services</returns>
        public static IMvcBuilder AddHyperMvc(this IServiceCollection services)
        {
            //add basic MVC feature
            var mvcBuilder = services.AddControllersWithViews();

            mvcBuilder.AddRazorRuntimeCompilation();

            //use cookie-based temp data provider
            mvcBuilder.AddCookieTempDataProvider(options =>
            {
                options.Cookie.Name = $"{HyperCookieDefaults.Prefix}{HyperCookieDefaults.TempDataCookie}";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            services.AddRazorPages();

            //MVC now serializes JSON with camel case names by default, use this code to avoid it
            mvcBuilder.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //set some options
            mvcBuilder.AddMvcOptions(options =>
            {
                options.ModelBinderProviders.Insert(0, new SearchModelBinderProvider());
            });

            //add fluent validation
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

            //register all available validators from Hyper assemblies
            var assemblies = mvcBuilder.PartManager.ApplicationParts
                .OfType<AssemblyPart>()
                .Where(part => part.Name.StartsWith("Hyper", StringComparison.InvariantCultureIgnoreCase))
                .Select(part => part.Assembly);

            services.AddValidatorsFromAssemblies(assemblies);

            //register controllers as services, it'll allow to override them
            mvcBuilder.AddControllersAsServices();

            return mvcBuilder;
        }

        /// <summary>
        /// Register custom RedirectResultExecutor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHyperRedirectResultExecutor(this IServiceCollection services)
        {
            //we use custom redirect executor as a workaround to allow using non-ASCII characters in redirect URLs
            //services.AddScoped<IActionResultExecutor<RedirectResult>, HyperRedirectResultExecutor>();
        }

        /// <summary>
        /// Add and configure MiniProfiler service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHyperMiniProfiler(this IServiceCollection services)
        {
            services.AddMiniProfiler(miniProfilerOptions =>
            {
                //use memory cache provider for storing each result
                ((MemoryCacheStorage)miniProfilerOptions.Storage).CacheDuration = TimeSpan.FromMinutes(60);
            });
        }

        /// <summary>
        /// Add and configure WebMarkupMin service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHyperWebMarkupMin(this IServiceCollection services)
        {
            services
                .AddWebMarkupMin(options =>
                {
                    options.AllowMinificationInDevelopmentEnvironment = true;
                    options.AllowCompressionInDevelopmentEnvironment = true;
                    options.DisableMinification = false;
                    options.DisableCompression = true;
                    options.DisablePoweredByHttpHeaders = true;
                })
                .AddHtmlMinification(options =>
                {
                    options.MinificationSettings.AttributeQuotesRemovalMode = HtmlAttributeQuotesRemovalMode.KeepQuotes;
                    options.CssMinifierFactory = new NUglifyCssMinifierFactory();
                    options.JsMinifierFactory = new NUglifyJsMinifierFactory();
                })
                .AddXmlMinification(options =>
                {
                    var settings = options.MinificationSettings;
                    settings.RenderEmptyTagsWithSpace = true;
                    settings.CollapseTagsWithoutContent = true;
                });
        }

        /// <summary>
        /// Adds WebOptimizer to the specified <see cref="IServiceCollection"/> and enables CSS and JavaScript minification.
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHyperWebOptimizer(this IServiceCollection services)
        {
            var EnableCssBundling = true;
            var EnableJavaScriptBundling = true;

            //add minification & bundling
            var cssSettings = new CssBundlingSettings
            {
                FingerprintUrls = false,
                Minify = EnableCssBundling
            };

            var codeSettings = new CodeBundlingSettings
            {
                Minify = EnableJavaScriptBundling,
                AdjustRelativePaths = false //disable this feature because it breaks function names that have "Url(" at the end
            };

            services.AddWebOptimizer(null, cssSettings, codeSettings);
        }

        /// <summary>
        /// Add and configure default HTTP clients
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHyperHttpClients(this IServiceCollection services)
        {
            //default client
            services.AddHttpClient(HyperHttpDefaults.DefaultHttpClient);

        }
    }
}
