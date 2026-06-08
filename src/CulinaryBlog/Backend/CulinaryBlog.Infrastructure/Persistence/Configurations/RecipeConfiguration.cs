using CulinaryBlog.Domain.Entities;
using CulinaryBlog.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CulinaryBlog.Infrastructure.Persistence.Configurations;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
	public void Configure(EntityTypeBuilder<Recipe> builder)
	{
		builder.HasKey(x => x.Id);

		builder.ToTable("Recipes");

		builder.Property(x => x.Title)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Slug)
			.IsRequired()
			.HasMaxLength(220);

		builder.Property(x => x.Description)
			.HasMaxLength(2000);

		builder.Property(x => x.ThumbnailUrl)
			.HasMaxLength(2048);

		// Enum → lưu dưới dạng string trong DB thay vì int
		// Lợi ích: dễ đọc dữ liệu trực tiếp trong DB, không cần lookup
		builder.Property(x => x.DifficultyLevel)
			.HasConversion<string>()
			.HasMaxLength(10);

		builder.Property(x => x.Status)
			.HasConversion<string>()
			.HasMaxLength(20)
			.HasDefaultValue(RecipeStatus.Draft);

		// AuthorId là FK đến ApplicationUser (sẽ thêm khi có Identity ở Lab 2)
		// Hiện tại khai báo như một Guid thuần
		builder.Property(x => x.AuthorId)
			.IsRequired();

		// Index cho các trường hay dùng để filter/sort
		builder.HasIndex(x => x.Slug)
			.IsUnique().HasDatabaseName("IX_Recipes_Slug");

		builder.HasIndex(x => x.Status)
			.HasDatabaseName("IX_Recipes_Status");

		builder.HasIndex(x => x.CategoryId)
			.HasDatabaseName("IX_Recipes_CategoryId");

		builder.HasIndex(x => x.AuthorId)
			.HasDatabaseName("IX_Recipes_AuthorId");

		// SearchVector: NpgsqlTsVector cho Full-Text Search
		// Cấu hình generated column sẽ được thực hiện ở Lab 3.
		// Ở đây chỉ khai báo để EF Core nhận biết property.
		builder.Property(x => x.SearchVector).HasColumnType("tsvector");

		// Quan hệ 1 recipe có N ingredients (Cascade delete: xóa Recipe -> xóa Ingredients)
		builder.HasMany(x => x.Ingredients)
			.WithOne(x => x.Recipe)
			.HasForeignKey(x => x.RecipeId)
			.OnDelete(DeleteBehavior.Cascade);

		// Quan hệ 1 recipe có N steps (Cascade delete: xóa Recipe -> xóa Steps)
		builder.HasMany(x => x.Steps)
			.WithOne(x => x.Recipe)
			.HasForeignKey(x => x.RecipeId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}