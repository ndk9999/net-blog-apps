using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TatBlog.Core.Settings;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.Services.Security;
using TatBlog.Services.Timing;

namespace TatBlog.WebApi.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddMemoryCache();

		builder.Services.AddDbContext<BlogDbContext>(options =>
			options.UseSqlServer(
				builder.Configuration
					.GetConnectionString("DefaultConnection")));

		builder.Services.AddScoped<ITimeProvider, LocalTimeProvider>();
		builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
		builder.Services.AddScoped<IBlogRepository, BlogRepository>();
		builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
		builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

		return builder;
	}

	public static WebApplicationBuilder ConfigureSettings(this WebApplicationBuilder builder)
	{
		builder.Services.Configure<JwtSettings>(
			builder.Configuration.GetSection(nameof(JwtSettings)));

		return builder;
	}

	public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
	{
		builder.Services.AddCors(options =>
		{
			options.AddPolicy("TatBlogApp", policyBuilder => 
				policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
		});

		return builder;
	}

	public static WebApplicationBuilder ConfigureSwaggerOpenApi(this WebApplicationBuilder builder)
	{
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(options =>
		{
			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
			{
				Scheme = "Bearer",
				BearerFormat = "JWT",
				Description = "JWT Authorization header using the bearer scheme",
				Name = "Authorization",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.Http
			});
			options.AddSecurityRequirement(new OpenApiSecurityRequirement()
			{
				{
					new OpenApiSecurityScheme()
					{
						Reference = new OpenApiReference()
						{
							Id = "Bearer",
							Type = ReferenceType.SecurityScheme
						}
					},
					new List<string>()
				}
			});
		});

		return builder;
	}

	public static WebApplicationBuilder ConfigureAuth(this WebApplicationBuilder builder)
	{
		builder.Services
			.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				var jwtSettings = builder.Configuration
					.GetSection(nameof(JwtSettings))
					.Get<JwtSettings>();

				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateActor = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuer = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtSettings.Issuer,
					ValidAudience = jwtSettings.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(jwtSettings.Secret))
				};
			});

		builder.Services.AddAuthorization(options =>
		{
			//// Configure authorization policy
			//options.AddPolicy("admin", policy => policy.RequireRole("Administrator"));
			//options.AddPolicy("editor", policy => policy.RequireRole("Editor"));
			//options.AddPolicy("author", policy => policy.RequireRole("Author"));
		});
		
		return builder;
	}

	public static WebApplication SetupRequestPipeline(this WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseStaticFiles();
		app.UseHttpsRedirection();

		app.UseCors("TatBlogApp");
		app.UseAuthentication();
		app.UseAuthorization();

		return app;
	}
}