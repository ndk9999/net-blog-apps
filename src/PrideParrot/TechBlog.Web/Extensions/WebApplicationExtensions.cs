﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechBlog.Core.Contexts;
using TechBlog.Core.Entities;
using TechBlog.Core.IdentityStores;
using TechBlog.Core.Repositories;

namespace TechBlog.Web.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplicationBuilder ConfigureMvc(this WebApplicationBuilder builder)
	{
		builder.Services.AddControllersWithViews();
		builder.Services.AddResponseCompression();
		builder.Services.AddMemoryCache();

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

	public static WebApplicationBuilder ConfigureIdentity(this WebApplicationBuilder builder)
	{
		builder.Services.ConfigureApplicationCookie(options =>
		{
			options.AccessDeniedPath = new PathString("/access-denied");
			options.LoginPath = new PathString("/login");
			options.ExpireTimeSpan = TimeSpan.FromDays(1);
			options.SlidingExpiration = true;
		});

		builder.Services
			.AddIdentity<Account, Role>(options =>
			{
				options.SignIn.RequireConfirmedAccount = true;
				options.Password.RequiredLength = 6;
				options.Password.RequireDigit = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = true;
			})
			.AddUserStore<AccountStore>()
			.AddRoleStore<RoleStore>()
			.AddDefaultTokenProviders();

		return builder;
	}

	public static WebApplication UseRequestPipeline(this WebApplication app)
	{
		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseResponseCompression();
		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthentication();
		app.UseAuthorization();

		return app;
	}

	public static IApplicationBuilder UseDataSeeder(this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();

		try
		{
			var dataSeeder = scope.ServiceProvider.GetService<IDataSeeder>();
			dataSeeder.Initialize();
		}
		catch (Exception ex)
		{
			var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
			logger.LogError(ex, "An error occurred when creating the database");
		}

		return app;
	}
}