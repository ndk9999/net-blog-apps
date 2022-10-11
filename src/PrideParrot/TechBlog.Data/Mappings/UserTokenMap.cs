using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechBlog.Core.Entities;

namespace TechBlog.Data.Mappings;

public class UserTokenMap : IEntityTypeConfiguration<UserToken>
{
	public void Configure(EntityTypeBuilder<UserToken> builder)
	{
		builder.ToTable("UserTokens");

		builder.HasKey(x => x.Id);
	}
}