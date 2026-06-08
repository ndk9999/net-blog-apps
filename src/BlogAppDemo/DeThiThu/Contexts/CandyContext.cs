using DeThiThu.Entities;
using DeThiThu.Mappings;
using Microsoft.EntityFrameworkCore;

namespace DeThiThu.Contexts;

public class CandyContext : DbContext
{
	public DbSet<Category> Categories { get; set; }

	public DbSet<Candy> Candies { get; set; }

	public DbSet<Subscriber> Subscribers { get; set; }

	public CandyContext(DbContextOptions<CandyContext> options)
		: base(options)
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer(
			"Server=PHUCNV;Database=CandyStore;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(
			typeof(CategoryMap).Assembly);
	}
}