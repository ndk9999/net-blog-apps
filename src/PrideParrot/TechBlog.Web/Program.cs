using TechBlog.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
	// Add services to the container.
	builder
		.ConfigureMvc()
		.ConfigureServices()
		.ConfigureAppSettings()
		.ConfigureIdentity()
		.ConfigureMapster();
}

var app = builder.Build();
{
	app.UseRequestPipeline();
	app.UseBlogRoutes();
	app.UseDataSeeder();
	
	app.Run();
}