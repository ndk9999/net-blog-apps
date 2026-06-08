namespace CulinaryBlog.Application.DTOs;

/// <summary>
/// DTO dùng cho detail view — bao gồm danh sách Recipes cơ bản.
/// </summary>
public record CategoryDetailDto(
	Guid Id,
	string Name,
	string Slug,
	string? Description,
	string? ImageUrl,
	int RecipeCount,
	DateTimeOffset CreatedAt,
	DateTimeOffset UpdatedAt
);