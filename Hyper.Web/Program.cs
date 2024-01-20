using Hyper.Core;
using Hyper.Data;
using Hyper.Services.ApplicationUsers;
using Hyper.Services.Appointments;
using Hyper.Services.Authentication;
using Hyper.Services.Employees;
using Hyper.Services.Messages;
using Hyper.Services.Notifications;
using Hyper.Services.Organisations;
using Hyper.Services.Patients;
using Hyper.Services.Products;
using Hyper.Services.Security;
using Hyper.Web.Areas.Admin.Factories.Appointments;
using Hyper.Web.Areas.Admin.Factories.Products;
using Hyper.Web.Areas.Admin.Factories.Reports;
using Hyper.Web.Areas.Admin.Factories.Stores;
using Hyper.Web.Infrastructure;
using Hyper.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.ConfigureApplicationSettings(builder);
services.ConfigureHttpContextAccessor();

builder.Host.UseDefaultServiceProvider(options =>
{
    //we don't validate the scopes, since at the app start and the initial configuration we need 
    //to resolve some services (registered as "scoped") through the root container
    options.ValidateScopes = false;
    options.ValidateOnBuild = true;
});

//compression
services.AddResponseCompression();

//middleware for bundling and minification of CSS and JavaScript files.
services.AddHyperWebOptimizer();

//add options feature
services.AddOptions();

//add HTTP sesion state feature
services.AddHttpSession();

//add default HTTP clients
services.AddHyperHttpClients();

//add anti-forgery
services.AddAntiForgery();

//add routing
services.AddRouting();

//add WebMarkupMin services to the services container
services.AddHyperWebMarkupMin();

//add MiniProfiler services
//services.AddHyperMiniProfiler();

//add data protection
//var dataProtectionKeysPath = Path.Combine(builder.Environment.WebRootPath , "~/App_Data/DataProtectionKeys");
//services.AddHyperDataProtection(dataProtectionKeysPath);

//add authentication
services.AddHyperAuthentication();

//add and configure MVC feature
services.AddHyperMvc();

services.AddWebEncoders();

//add custom redirect result executor
services.AddHyperRedirectResultExecutor();

//application layers
//data layer
services.AddScoped<IHyperDbProvider, HyperObjectContext>();
services.AddScoped(typeof(IRepository<>), typeof(StoreRepository<>));

//service Layer
services.AddScoped<ISmsSender, SmsSender>();
services.AddScoped<IEmailSender, EmailSender>();
services.AddScoped<IEncryptionService, EncryptionService>();
services.AddScoped<IApplicationUserService, ApplicationUserService>();
services.AddScoped<IAuthenticationService, CookieAuthenticationService>();
services.AddScoped<IStoreService, StoreService>();
services.AddScoped<IEmployeeService, EmployeeService>();
services.AddScoped<IPatientService, PatientService>();
services.AddScoped<IProductService, ProductService>();
services.AddScoped<IAppointmentService, AppointmentService>();

//web Layer
//admin
services.AddScoped<IProductFactory, ProductFactory>();
services.AddScoped<IAppointmentFactory, AppointmentFactory>();
services.AddScoped<IStoreFactory, StoreFactory>();
services.AddScoped<IReportFactory, ReportFactory>();

//contexts
services.AddScoped<IWorkContext, WebWorkContext>();
services.AddScoped<IStoreContext, WebStoreContext>();

services.ConfigureAutoMapperProfiles();

var application = builder.Build();

//request pipeline
application.UseHyperExceptionHandler(builder.Environment.IsDevelopment());

//handle 400 errors (bad request)
application.UseBadRequestResult();

//handle 404 errors (not found)
application.UsePageNotFound();

//use response compression before UseHyperStaticFiles to support compress for it
application.UseHyperResponseCompression();

//WebOptimizer should be placed before configuring static files
application.UseHyperWebOptimizer();

//use static files feature
application.UseHyperStaticFiles();

//use HTTP session
application.UseSession();

//use WebMarkupMin
application.UseHyperWebMarkupMin();

//use MiniProfiler must come before UseHyperEndpoints
//application.UseMiniProfiler();

//Add the RoutingMiddleware
application.UseRouting();

//configure authentication
application.UseHyperAuthentication();

//Add the Authorization middleware
application.UseAuthorization();

//Endpoints routing
application.UseHyperEndpoints();
 
application.Run();