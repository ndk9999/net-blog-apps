using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechBlog.Core.Entities;

namespace TechBlog.Data.Mappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
	public void Configure(EntityTypeBuilder<Category> builder)
	{
		builder.ToTable("Categories");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.HasMaxLength(50)
			.IsRequired();
		
		builder.Property(x => x.Description)
			.HasMaxLength(500);

		builder.Property(x => x.UrlSlug)
			.HasMaxLength(50)
			.IsRequired();
	}
}