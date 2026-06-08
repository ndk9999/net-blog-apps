using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.Services.IdentityStores;
using TatBlog.Services.Media;
using TatBlog.Services.Security;
using TatBlog.Services.Timing;

namespace TatBlog.WebApp.Extensions;

public static class WebApplicationExtensions
{
	// Thêm các dịch vụ được yêu cầu bởi MVC Framework
	public static WebApplicationBuilder ConfigureMvc(
		this WebApplicationBuilder builder)
	{
		builder.Services.AddControllersWithViews();
		//builder.Services.AddResponseCaching();
		//builder.Services.AddOutputCache();
		//builder.Services.AddResponseCompression();
		builder.Services.AddMemoryCache();

		return builder;
	}

	public static WebApplicationBuilder ConfigureNLog(this WebApplicationBuilder builder)
	{
		builder.Logging.ClearProviders();
		builder.Host.UseNLog();

		return builder;
	}

	// Đăng ký các dịch vụ với DI Container
	public static WebApplicationBuilder ConfigureServices(
		this WebApplicationBuilder builder)
	{
		builder.Services.AddDbContext<BlogDbContext>(options =>
			options.UseSqlServer(
				builder.Configuration
					.GetConnectionString("DefaultConnection")));

		builder.Services.AddHttpContextAccessor();
		builder.Services.AddHttpClient();

		builder.Services.AddScoped<ITimeProvider, LocalTimeProvider>();
		builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
		builder.Services.AddScoped<IBlogRepository, BlogRepository>();
		builder.Services.AddScoped<IAuthService, AuthService>();
		builder.Services.AddScoped<IDataSeeder, DataSeeder>();

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
			options.AccessDeniedPath = new PathString("/admin/dashboard/denied");
			options.LoginPath = new PathString("/admin/dashboard/login");
			options.ExpireTimeSpan = TimeSpan.FromDays(1);
			options.SlidingExpiration = true;
			options.Cookie.Name = "TatBlog.Identity";
			options.Cookie.HttpOnly = true;
		});

		return builder;
	}

	// Cấu hình HTTP Request pipeline
	public static WebApplication UseRequestPipeline(
		this WebApplication app)
	{
		// Thêm middleware để hiển thị thông báo lỗi
		if (app.Environment.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseExceptionHandler("/Blog/Error");

			// Thêm middleware cho việc áp dụng HSTS (thêm header
			// Strict-Transport-Security vào HTTP Response).
			app.UseHsts();
		}

		// Thêm middleware để tự động nén HTTP response
		//app.UseResponseCompression();

		// Thêm middleware để chuyển hướng HTTP sang HTTPS
		app.UseHttpsRedirection();

		// Thêm middleware phục vụ các yêu cầu liên quan
		// tới các tập tin nội dung tĩnh như hình ảnh, css, ...
		app.UseStaticFiles();

		// Thêm middleware lựa chọn endpoint phù hợp nhất
		// để xử lý một HTTP request.
		app.UseRouting();

		// Thêm middleware để xác thực người dùng và
		// kiểm tra quyền truy cập tới các endpoint.
		app.UseAuthentication();
		app.UseAuthorization();

		//app.UseResponseCaching();
		//app.UseOutputCache();

		return app;
	}

	// Thêm dữ liệu mẫu vào CSDL
	public static IApplicationBuilder UseDataSeeder(
		this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();

		try
		{
			scope.ServiceProvider
				.GetRequiredService<IDataSeeder>()
				.Initialize();
		}
		catch (Exception ex)
		{
			scope.ServiceProvider
				.GetRequiredService<ILogger<Program>>()
				.LogError(ex, "Could not insert data into database");
		}

		return app;
	}
}