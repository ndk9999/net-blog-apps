using CulinaryBlog.Application.Contracts.Persistence;
using CulinaryBlog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CulinaryBlog.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services, IConfiguration configuration)
	{
		// Đăng ký PostgreSQL với Entity Framework Core
		services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.UseNpgsql(
				configuration.GetConnectionString("DefaultConnection"),
				npgsqlOptions => npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

			// Chỉ bật sensitive data logging trong Development để xem giá trị parameters trong SQL logs
			// Không bao giờ bật trong Production!
		});

		// Đăng ký ApplicationDbContext như một implementation của IApplicationDbContext
		// Scoped: mỗi HTTP request có một instance riêng (lifecycle của DbContext)
		services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

		return services;
	}
}
