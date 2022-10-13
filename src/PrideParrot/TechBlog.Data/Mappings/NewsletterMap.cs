using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechBlog.Core.Entities;

namespace TechBlog.Data.Mappings;

public class NewsletterMap : IEntityTypeConfiguration<Newsletter>
{
	public void Configure(EntityTypeBuilder<Newsletter> builder)
	{
		builder.ToTable("Newsletters");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Status)
			.HasColumnType("int")
			.IsRequired();

		builder.Property(x => x.Notes)
			.HasMaxLength(500);

		builder.HasOne(x => x.Post)
			.WithMany()
			.HasForeignKey(x => x.PostId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.Subscriber)
			.WithMany()
			.HasForeignKey(x => x.SubscriberId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}