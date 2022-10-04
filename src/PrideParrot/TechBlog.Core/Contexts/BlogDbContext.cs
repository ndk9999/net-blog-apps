using Microsoft.EntityFrameworkCore;
using TechBlog.Core.Entities;

namespace TechBlog.Core.Contexts;

public class BlogDbContext : DbContext
{
	public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(Post).Assembly);
	}
}