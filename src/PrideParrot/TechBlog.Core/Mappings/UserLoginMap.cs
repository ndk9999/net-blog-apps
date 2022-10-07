using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TechBlog.Core.Entities;

namespace TechBlog.Core.Mappings;

public class UserLoginMap : IEntityTypeConfiguration<UserLogin>
{
	public void Configure(EntityTypeBuilder<UserLogin> builder)
	{
		builder.ToTable("UserLogins");

		builder.HasKey(login => login.Id);
	}
}
