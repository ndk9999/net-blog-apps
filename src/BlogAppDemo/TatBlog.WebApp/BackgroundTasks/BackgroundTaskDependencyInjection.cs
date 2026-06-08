namespace TatBlog.WebApp.BackgroundTasks;

public static class BackgroundTaskDependencyInjection
{
	public static WebApplicationBuilder ConfigureBackgroundTasks(
		this WebApplicationBuilder builder)
	{
		builder.Services.AddHostedService<TimeLogTask>();

		return builder;
	}
}