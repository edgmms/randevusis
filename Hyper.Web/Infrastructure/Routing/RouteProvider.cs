using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Hyper.Web.Infrastructure.Routing
{
	public class RouteProvider
	{
		/// <summary>
		/// Register routes
		/// </summary>
		/// <param name="endpointRouteBuilder">Route builder</param>
		public static void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
		{
			//areas
			endpointRouteBuilder.MapControllerRoute(name: "areaRoute",
				pattern: $"{{area:exists}}/{{controller=Home}}/{{action=Index}}/{{id?}}");

			//home page
			endpointRouteBuilder.MapControllerRoute(name: "Admin",
				pattern: "/Admin",
				defaults: new { area = "Admin", controller = "Home", action = "Index" });

			//home page
			endpointRouteBuilder.MapControllerRoute(name: "Homepage",
				pattern: "/",
				defaults: new { controller = "Home", action = "Index" });

			//login
			endpointRouteBuilder.MapControllerRoute(name: "Login",
				pattern: "/giris",
				defaults: new { controller = "ApplicationUser", action = "Login" });

			//register
			endpointRouteBuilder.MapControllerRoute(name: "Register",
				pattern: "/kayit",
				defaults: new { controller = "ApplicationUser", action = "Register" });

			//logout
			endpointRouteBuilder.MapControllerRoute(name: "Logout",
				pattern: "/cikis",
				defaults: new { controller = "ApplicationUser", action = "Logout" });

			endpointRouteBuilder.MapControllerRoute(name: "PasswordRecovery",
			 pattern: "/sifremi-kurtar",
			 defaults: new { controller = "ApplicationUser", action = "PasswordRecovery" });

			//default route
			endpointRouteBuilder.MapControllerRoute(name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}"
			);
		}
	}
}
