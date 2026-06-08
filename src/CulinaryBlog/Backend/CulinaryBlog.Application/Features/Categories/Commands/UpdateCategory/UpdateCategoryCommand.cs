using CulinaryBlog.Application.DTOs;
using MediatR;

namespace CulinaryBlog.Application.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(
	Guid Id,
	string Name,
	string? Description,
	string? ImageUrl
) : IRequest<CategoryDto>;