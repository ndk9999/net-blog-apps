using CulinaryBlog.Application.Contracts.Persistence;
using CulinaryBlog.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaryBlog.Application.Features.Categories.Queries.GetCategoryBySlug;

public class GetCategoryBySlugQueryHandler(IApplicationDbContext dbContext)
	: IRequestHandler<GetCategoryBySlugQuery, CategoryDetailDto?>
{
	public async Task<CategoryDetailDto?> Handle(
		GetCategoryBySlugQuery request, 
		CancellationToken cancellationToken)
	{
		var category = await dbContext.Categories
			.AsNoTracking()
			.Include(x => x.Recipes)
			.FirstOrDefaultAsync(c => c.Slug == request.Slug, cancellationToken: cancellationToken);

		return category == null ? null : new CategoryDetailDto(
			category.Id,
			category.Name,
			category.Slug,
			category.Description,
			category.ImageUrl,
			category.Recipes.Count,
			category.CreatedAt,
			category.UpdatedAt);
	}
}