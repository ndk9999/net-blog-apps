using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechBlog.Core.Entities;

namespace TechBlog.Core.Mappings;

public class RoleClaimMap : IEntityTypeConfiguration<RoleClaim>
{
	public void Configure(EntityTypeBuilder<RoleClaim> builder)
	{
		builder.ToTable("RoleClaims");

		builder.HasKey(x => x.Id);

	}
}