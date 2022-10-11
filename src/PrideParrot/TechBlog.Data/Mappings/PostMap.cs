using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechBlog.Core.Entities;

namespace TechBlog.Data.Mappings;

public class PostMap : IEntityTypeConfiguration<Post>
{
	public void Configure(EntityTypeBuilder<Post> builder)
	{
		builder.ToTable("Posts");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Title)
			.HasMaxLength(500)
			.IsRequired();

		builder.Property(x => x.ShortDescription)
			.HasMaxLength(5000)
			.IsRequired();

		builder.Property(x => x.Description)
			.HasMaxLength(5000)
			.IsRequired();

		builder.Property(x => x.UrlSlug)
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(x => x.Meta)
			.HasMaxLength(1000)
			.IsRequired();

		builder.HasOne(x => x.Category)
			.WithMany(x => x.Posts)
			.HasForeignKey(x => x.CategoryId)
			.HasConstraintName("FK_Posts_Categories")
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasMany(x => x.Tags)
			.WithMany(x => x.Posts)
			.UsingEntity(x => x.ToTable("PostTags"));
	}
}