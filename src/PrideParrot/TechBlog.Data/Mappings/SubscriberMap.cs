using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechBlog.Core.Entities;

namespace TechBlog.Data.Mappings;

public class SubscriberMap : IEntityTypeConfiguration<Subscriber>
{
	public void Configure(EntityTypeBuilder<Subscriber> builder)
	{
		builder.ToTable("Subscribers");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Email)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(x => x.UnsubscribedReason)
			.HasMaxLength(500);

		builder.Property(x => x.Notes)
			.HasMaxLength(500);

		builder.Property(x => x.Voluntary)
			.HasDefaultValue(true);
	}
}