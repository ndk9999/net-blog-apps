using CulinaryBlog.Application.Common.Models;
using CulinaryBlog.Application.DTOs;
using MediatR;

namespace CulinaryBlog.Application.Features.Categories.Queries.GetCategories;

/// <summary>
/// Truy vấn lấy danh sách Category có phân trang và tìm kiếm.
/// [AsParameters] trong Minimal API cho phép bind query string tự động.
/// </summary>
public record GetCategoriesQuery(
	int PageNumber = 1,
	int PageSize = 10,
	string? Search = null,          // Tìm theo Name hoặc Description
	string? SortBy = "name",        // name | createdAt | recipeCount
	bool Descending = false
) : IRequest<PaginatedResult<CategoryDto>>;