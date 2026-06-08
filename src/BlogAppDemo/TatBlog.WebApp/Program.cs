using TatBlog.WebApp.BackgroundTasks;
using TatBlog.WebApp.Extensions;
using TatBlog.WebApp.Mapsters;

var builder = WebApplication.CreateBuilder(args);
{
	builder
		.ConfigureMvc()
		.ConfigureIdentity()
		.ConfigureServices()
		//.ConfigureBackgroundTasks()
		.ConfigureMapster();
}

var app = builder.Build();
{
	app.UseRequestPipeline();
	app.UseBlogRoutes();
	app.UseDataSeeder();
}

app.Run();
