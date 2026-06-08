using DeThiThu.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeThiThu.Mappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
	public void Configure(EntityTypeBuilder<Category> builder)
	{
		builder.ToTable("Categories");

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Name)
			.HasMaxLength(50)
			.IsRequired();
		
		builder.Property(p => p.ShowOnMenu)
			.IsRequired()
			.HasDefaultValue(false);
	}
}