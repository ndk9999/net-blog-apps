using FluentEmail.Core.Defaults;
using FluentEmail.Core.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TechBlog.Core.Contexts;
using TechBlog.Core.Entities;
using TechBlog.Core.IdentityStores;
using TechBlog.Core.Repositories;
using TechBlog.Core.Settings;
using TechBlog.Web.Mapsters;
using TechBlog.Web.Providers;
using static TechBlog.Core.Constants.Default;

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

		builder.Services.AddHttpContextAccessor();
		builder.Services.AddHttpClient();

		builder.Services.AddScoped<IDataSeeder, DataSeeder>();
		builder.Services.AddScoped<IBlogRepository, BlogRepository>();
		builder.Services.AddScoped<IAuthProvider, AuthProvider>();
		builder.Services.AddScoped<ICaptchaProvider, GoogleRecaptchaProvider>();

		return builder;
	}

	public static WebApplicationBuilder ConfigureAppSettings(this WebApplicationBuilder builder)
	{
		builder.Services.Configure<RecaptchaSettings>(
			builder.Configuration.GetSection(RecaptchaSettings.ConfigSectionName));

		builder.Services.Configure<MailingSettings>(
			builder.Configuration.GetSection(MailingSettings.ConfigSectionName));

		return builder;
	}

	public static WebApplicationBuilder ConfigureIdentity(this WebApplicationBuilder builder)
	{
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

		// This must be called after AddIdentity
		builder.Services.ConfigureApplicationCookie(options =>
		{
			options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
			options.AccessDeniedPath = new PathString("/access-denied");
			options.LoginPath = new PathString("/login");
			options.ExpireTimeSpan = TimeSpan.FromDays(1);
			options.SlidingExpiration = true;
			options.Cookie.Name = "TechBlog.Identity";
			options.Cookie.HttpOnly = true;
		});

		return builder;
	}

	public static WebApplicationBuilder ConfigureMapster(this WebApplicationBuilder builder)
	{
		var config = TypeAdapterConfig.GlobalSettings;
		config.Scan(typeof(MapsterConfiguration).Assembly);

		builder.Services.AddSingleton(config);
		builder.Services.AddScoped<IMapper, ServiceMapper>();

		return builder;
	}

	public static WebApplicationBuilder ConfigureFluentEmail(this WebApplicationBuilder builder)
	{
		var mailingSettings = builder.Configuration
			.GetSection(MailingSettings.ConfigSectionName)
			.Get<MailingSettings>();

		if (mailingSettings.MailingService == MailingServiceNames.Smtp)
		{
			var smtpSettings = builder.Configuration
				.GetSection(SmtpSettings.ConfigSectionName)
				.Get<SmtpSettings>();

			builder.Services
				.AddFluentEmail(mailingSettings.SenderAddress, mailingSettings.SenderName)
				.AddSmtpSender(
					smtpSettings.Host, 
					smtpSettings.Port, 
					smtpSettings.Username, 
					smtpSettings.Password);
		}
		else
		{
			var sendGridSettings = builder.Configuration
				.GetSection(SendGridSettings.ConfigSectionName)
				.Get<SendGridSettings>();

			builder.Services
				.AddFluentEmail(mailingSettings.SenderAddress, mailingSettings.SenderName)
				.AddSendGridSender(sendGridSettings.ApiKey, true);
		}

		builder.Services.TryAdd(ServiceDescriptor.Singleton<ITemplateRenderer, ReplaceRenderer>(_ => new ReplaceRenderer()));

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