using DeThiThu.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeThiThu.Mappings;

public class CandyMap : IEntityTypeConfiguration<Candy>
{
	public void Configure(EntityTypeBuilder<Candy> builder)
	{
		builder.ToTable("Candies");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Name)
			.HasMaxLength(150)
			.IsRequired();
		
		builder.Property(p => p.Price).HasColumnType("decimal(18,2)");

		builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Cascade);
	}
}