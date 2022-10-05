namespace TechBlog.Web.Extensions;

public static class RouteExtensions
{
	public static IEndpointRouteBuilder ConfigureBlogRoutes(this IEndpointRouteBuilder endpoints)
	{
		endpoints.MapControllerRoute(
			name: "contact-me",
			pattern: "contact",
			defaults: new { controller = "Home", action = "Contact" });

		endpoints.MapControllerRoute(
			name: "about-me",
			pattern: "about-me",
			defaults: new { controller = "Home", action = "About" });

		endpoints.MapControllerRoute(
			name: "default",
			pattern: "{action}",
			defaults: new { controller = "Blog", action = "Posts" });

		return endpoints;
	}
}