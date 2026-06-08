using DeThiThu.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeThiThu.Mappings;

public class SubscriberMap : IEntityTypeConfiguration<Subscriber>
{
	public void Configure(EntityTypeBuilder<Subscriber> builder)
	{
		builder.ToTable("Subscribers");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Email).HasMaxLength(150);
	}
}