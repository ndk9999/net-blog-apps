using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechBlog.Core.Entities;

namespace TechBlog.Core.Mappings;

public class AccountMap : IEntityTypeConfiguration<Account>
{
	public void Configure(EntityTypeBuilder<Account> builder)
	{
		builder.ToTable("Accounts");

		builder.HasKey(x => x.Id);

		builder.Property(u => u.UserName).HasMaxLength(50);
		builder.Property(u => u.NormalizedUserName).HasMaxLength(50);
		builder.Property(u => u.Email).HasMaxLength(256);
		builder.Property(u => u.NormalizedEmail).HasMaxLength(256);
		builder.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

		builder.HasIndex(u => u.NormalizedUserName).HasDatabaseName("IX_Accounts_NormalizedUserName").IsUnique();
		builder.HasIndex(u => u.NormalizedEmail).HasDatabaseName("IX_Accounts_NormalizedUserEmail").IsUnique();

		builder.HasMany<UserInRole>().WithOne(x => x.Account).HasForeignKey(x => x.UserId).IsRequired();
		builder.HasMany<UserClaim>().WithOne().HasForeignKey(x => x.UserId).IsRequired();
		builder.HasMany<UserLogin>().WithOne().HasForeignKey(x => x.UserId).IsRequired();
		builder.HasMany<UserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
	}
}