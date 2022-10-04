using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechBlog.Core.Entities;

namespace TechBlog.Core.Mappings;

public class TagMap : IEntityTypeConfiguration<Tag>
{
	public void Configure(EntityTypeBuilder<Tag> builder)
	{
		builder.ToTable("Tags");

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