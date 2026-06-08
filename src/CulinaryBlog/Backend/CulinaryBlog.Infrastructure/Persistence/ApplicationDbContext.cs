using CulinaryBlog.Application.Contracts.Persistence;
using CulinaryBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CulinaryBlog.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
	public DbSet<Category> Categories => Set<Category>();
	
	public DbSet<Recipe> Recipes => Set<Recipe>();
	
	public DbSet<Ingredient> Ingredients => Set<Ingredient>();

	public DbSet<RecipeStep> RecipeSteps => Set<RecipeStep>();

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Áp dụng tất cả cấu hình từ assembly hiện tại
		// ApplyConfigurationsFromAssembly tự động tìm tất cả
		// IEntityTypeConfiguration<T> trong Infrastructure assembly.
		// Không cần gọi thủ công từng modelBuilder.ApplyConfiguration(new Xxx()).
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
	}
}