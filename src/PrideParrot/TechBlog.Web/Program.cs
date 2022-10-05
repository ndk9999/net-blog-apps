using TechBlog.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
	// Add services to the container.
	builder
		.ConfigureMvc()
		.ConfigureServices();
}

var app = builder.Build();
{
	app.ConfigureRequestPipeline();
	app.ConfigureBlogRoutes();
	app.ConfigureDataSeeder();
	
	app.Run();
}