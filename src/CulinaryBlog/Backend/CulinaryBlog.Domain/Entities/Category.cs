using CulinaryBlog.Domain.Extensions;

namespace CulinaryBlog.Domain.Entities;

/// <summary>
/// Danh mục phân loại công thức nấu ăn.
/// Ví dụ: "Món Việt", "Món Á", "Bánh ngọt", "Đồ uống".
/// </summary>
public class Category : BaseEntity<Guid>
{
	// Tên danh mục, ví dụ: "Desserts", "Main Courses", "Appetizers".
	// Name và Slug là 2 trường dữ liệu quan trọng để hiển thị và định danh danh mục trên UI.
	// Chúng phải khác null và không được để trống, đảm bảo rằng mỗi danh mục đều có tên và slug hợp lệ.
	public string Name { get; private set; } = string.Empty;

	// URL-friendly unique identifier được sinh ra từ Name (e.g., "Desserts" → "desserts").
	public string Slug { get; private set; } = string.Empty;

	public string? Description { get; private set; }
	public string? ImageUrl { get; private set; }

	// Navigation property: Một Category có thể có nhiều Recipe.
	// ICollection<Recipe> thể hiện quan hệ 1 category : N recipes.
	// EF Core sẽ lazy/eager load danh sách công thức khi truy vấn Category nếu cần thiết.
	public ICollection<Recipe> Recipes { get; private set; } = [];

	// ── EF Core parameterless constructor (protected) ──────────────────
	// EF Core cần constructor này để tái tạo lại đối tượng từ database.
	// private set trên properties đảm bảo chỉ thay đổi qua Domain methods.
	protected Category()
	{
	}

	// ── Factory Method — đảm bảo Category luôn ở trạng thái hợp lệ ─────
	public static Category Create(string name, string? description = null, string? imageUrl = null)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Category name cannot be null or empty.", nameof(name));
		
		var category = new Category
		{
			Id = Guid.NewGuid(),
			Name = name.Trim(),
			Slug = name.ToSlug(),          // Tạo slug: thường là lowercase, thay khoảng trắng bằng dấu gạch ngang, loại bỏ ký tự đặc biệt.
			Description = description?.Trim(),
			ImageUrl = imageUrl?.Trim(),
			CreatedAt = DateTimeOffset.UtcNow,
			UpdatedAt = DateTimeOffset.UtcNow
		};

		return category;
	}

	// ── Domain Method — Update thông qua phương thức có tên rõ ràng ─────
	public void Update(string name, string? description = null, string? imageUrl = null)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Category name cannot be null or empty.", nameof(name));

		Name = name.Trim();
		Slug = name.ToSlug(); // Cập nhật slug nếu tên thay đổi
		Description = description?.Trim();
		ImageUrl = imageUrl?.Trim();
		UpdatedAt = DateTimeOffset.UtcNow;
	}
}