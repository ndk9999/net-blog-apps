using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechBlog.Core.Entities;

namespace TechBlog.Data.Mappings;

public class UserInRoleMap : IEntityTypeConfiguration<UserInRole>
{
	public void Configure(EntityTypeBuilder<UserInRole> builder)
	{
		builder.ToTable("UserInRoles");

		builder.HasKey(r => r.Id);
	}
}
