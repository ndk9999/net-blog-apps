using TechBlog.Web.Extensions;
using TechBlog.Web.Mapsters;
using TechBlog.Web.Validations;

var builder = WebApplication.CreateBuilder(args);
{
	// Add services to the container.
	builder
		.ConfigureMvc()
		.ConfigureServices()
		.ConfigureAppSettings()
		.ConfigureIdentity()
		.ConfigureMapster()
		.ConfigureFluentEmail()
		.ConfigureFluentValidation();
}

var app = builder.Build();
{
	app.UseRequestPipeline();
	app.UseBlogRoutes();
	app.UseDataSeeder();
	
	app.Run();
}