using Microsoft.EntityFrameworkCore;
using TechBlog.Core.Contexts;
using TechBlog.Core.Repositories;

namespace TechBlog.Web.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplicationBuilder ConfigureMvc(this WebApplicationBuilder builder)
	{
		builder.Services.AddControllersWithViews();

		return builder;
	}

	public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddDbContext<BlogDbContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

		builder.Services.AddScoped<IDataSeeder, DataSeeder>();
		builder.Services.AddScoped<IBlogRepository, BlogRepository>();

		return builder;
	}

	public static WebApplication ConfigureRequestPipeline(this WebApplication app)
	{
		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthorization();

		return app;
	}

	public static IApplicationBuilder ConfigureDataSeeder(this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();

		try
		{
			var dbContext = scope.ServiceProvider.GetService<BlogDbContext>();
			var dataSeeder = scope.ServiceProvider.GetService<IDataSeeder>();

			dataSeeder.Initialize(dbContext);
		}
		catch (Exception ex)
		{
			var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
			logger.LogError(ex, "An error occurred when creating the database");
		}

		return app;
	}
}