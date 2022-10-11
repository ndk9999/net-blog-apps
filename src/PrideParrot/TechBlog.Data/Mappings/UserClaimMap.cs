using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechBlog.Core.Entities;

namespace TechBlog.Data.Mappings;

public class UserClaimMap : IEntityTypeConfiguration<UserClaim>
{
	public void Configure(EntityTypeBuilder<UserClaim> builder)
	{
		builder.ToTable("UserClaims");

		builder.HasKey(x => x.Id);
	}
}