using TechBlog.Services.Blogs;

namespace TechBlog.Web.BackgroundTasks;

public static class BackgroundTaskDependencyInjection
{
	public static WebApplicationBuilder ConfigureBackgroundTasks(this WebApplicationBuilder builder)
	{
		builder.Services.AddSingleton<INewsletterQueue, NewsletterQueue>();
		builder.Services.AddHostedService<SendNewsletterTask>();

		return builder;
	}
}