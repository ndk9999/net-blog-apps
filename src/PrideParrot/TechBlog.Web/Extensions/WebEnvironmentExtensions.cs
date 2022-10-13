using NuGet.Protocol.Plugins;

namespace TechBlog.Web.Extensions;

public static class WebEnvironmentExtensions
{
	public static string GetEmailTemplateFullPath(
		this IWebHostEnvironment environment, string templateName)
	{
		return Path.Combine(environment.WebRootPath, "templates", "emails", templateName);
	}
}