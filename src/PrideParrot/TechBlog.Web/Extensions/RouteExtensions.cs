namespace TechBlog.Web.Extensions;

public static class RouteExtensions
{
	public static IEndpointRouteBuilder UseBlogRoutes(this IEndpointRouteBuilder endpoints)
	{
		endpoints.MapControllerRoute(
			name: "sign-in",
			pattern: "login",
			defaults: new { controller = "Admin", action = "Login" });

		endpoints.MapControllerRoute(
			name: "sign-out",
			pattern: "logout",
			defaults: new { controller = "Admin", action = "Logout" });

		endpoints.MapControllerRoute(
			name: "dashboard",
			pattern: "dashboard",
			defaults: new { controller = "Admin", action = "Dashboard" });

		endpoints.MapControllerRoute(
			name: "manage",
			pattern: "manage/{action}/{id?}",
			defaults: new { controller = "Admin", action = "Posts" });

		endpoints.MapControllerRoute(
			name: "contact-me",
			pattern: "contact",
			defaults: new { controller = "Home", action = "Contact" });

		endpoints.MapControllerRoute(
			name: "about-me",
			pattern: "about-me",
			defaults: new { controller = "Home", action = "About" });

		endpoints.MapControllerRoute(
			name: "subscribe",
			pattern: "subscribe",
			defaults: new { controller = "Home", action = "Subscribe" });

		endpoints.MapControllerRoute(
			name: "unsubscribe",
			pattern: "unsubscribe",
			defaults: new { controller = "Home", action = "Unsubscribe" });

		endpoints.MapControllerRoute(
			name: "rss-feed",
			pattern: "rss",
			defaults: new { controller = "Home", action = "Rss" });

		endpoints.MapControllerRoute(
			name: "posts-by-category",
			pattern: "category/{slug}",
			defaults: new { controller = "Blog", action = "Category" });

		endpoints.MapControllerRoute(
			name: "posts-by-tag",
			pattern: "tag/{slug}",
			defaults: new { controller = "Blog", action = "Tag" });

		endpoints.MapControllerRoute(
			name: "archives",
			pattern: "archive/{year}/{month}",
			defaults: new { controller = "Blog", action = "Archive" });

		endpoints.MapControllerRoute(
			name: "single-post",
			pattern: "post/{year}/{month}/{day}/{slug}",
			defaults: new { controller = "Blog", action = "Post" });

		endpoints.MapControllerRoute(
			name: "default",
			pattern: "{action}",
			defaults: new { controller = "Blog", action = "Posts" });

		return endpoints;
	}
}