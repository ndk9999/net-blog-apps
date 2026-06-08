namespace CulinaryBlog.Application.DTOs;

/// <summary>
/// DTO dùng cho danh sách (list view) — không bao gồm chi tiết Recipes.
/// </summary>
public record CategoryDto(
    Guid Id,
    string Name,
	string Slug,
	string? Description,
	string? ImageUrl,
	int RecipeCount,    // Số lượng công thức thuộc danh mục này
	DateTimeOffset CreatedAt
);