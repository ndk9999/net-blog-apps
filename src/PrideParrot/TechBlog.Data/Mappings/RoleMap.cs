using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechBlog.Core.Entities;

namespace TechBlog.Data.Mappings;

public class RoleMap : IEntityTypeConfiguration<Role>
{
	public void Configure(EntityTypeBuilder<Role> builder)
	{
		builder.ToTable("Roles");

		builder.HasKey(x => x.Id);

		builder.Property(u => u.Name).HasMaxLength(50);
		builder.Property(u => u.NormalizedName).HasMaxLength(50);
		builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

		builder.HasIndex(r => r.NormalizedName).HasDatabaseName("IX_Roles_NormalizedRoleName").IsUnique();

		builder.HasMany<UserInRole>().WithOne(x => x.Role).HasForeignKey(x => x.RoleId).IsRequired();
		builder.HasMany<RoleClaim>().WithOne().HasForeignKey(x => x.RoleId).IsRequired();
	}
}
