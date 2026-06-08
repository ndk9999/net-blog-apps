using CulinaryBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CulinaryBlog.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
	public void Configure(EntityTypeBuilder<Category> builder)
	{
		builder.HasKey(x => x.Id);

		// Tên bảng rõ ràng - tránh phụ thuộc EF naming conventions
		builder.ToTable("Categories");

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(x => x.Slug)
			.IsRequired()
			.HasMaxLength(120);

		builder.Property(x => x.Description)
			.HasMaxLength(500);

		builder.Property(x => x.ImageUrl)
			.HasMaxLength(2048);

		// Slug phải là UNIQUE — dùng để lookup thay vì Guid trong URL
		builder.HasIndex(x => x.Slug)
			.IsUnique()
			.HasDatabaseName("IX_Categories_Slug");

		// Quan hệ 1-n với Recipe
		// WillCascadeOnDelete false — không xóa Recipe khi xóa Category
		builder.HasMany(x => x.Recipes)
			.WithOne(r => r.Category)
			.HasForeignKey(r => r.CategoryId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}