﻿namespace TechBlog.Web.Extensions;

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
			name: "posts-by-category",
			pattern: "category/{slug}",
			defaults: new { controller = "Blog", action = "Category" });

		endpoints.MapControllerRoute(
			name: "posts-by-tag",
			pattern: "tag/{slug}",
			defaults: new { controller = "Blog", action = "Tag" });

		endpoints.MapControllerRoute(
			name: "single-post",
			pattern: "archive/{year}/{month}/{slug}",
			defaults: new { controller = "Blog", action = "Post" });

		endpoints.MapControllerRoute(
			name: "default",
			pattern: "{action}",
			defaults: new { controller = "Blog", action = "Posts" });

		return endpoints;
	}
}