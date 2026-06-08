using CulinaryBlog.Application.DTOs;
using MediatR;

namespace CulinaryBlog.Application.Features.Categories.Commands.CreateCategory;

/// <summary>
/// Command tạo danh mục mới.
/// record là lựa chọn hoàn hảo cho Command/Query: immutable, value equality.
/// </summary>
public record CreateCategoryCommand(
	string Name,
	string? Description,
	string? ImageUrl
) : IRequest<CategoryDto>;