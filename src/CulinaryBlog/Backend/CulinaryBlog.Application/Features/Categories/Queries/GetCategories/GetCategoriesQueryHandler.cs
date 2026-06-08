using CulinaryBlog.Application.Common.Models;
using CulinaryBlog.Application.Contracts.Persistence;
using CulinaryBlog.Application.DTOs;
using CulinaryBlog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaryBlog.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler(IApplicationDbContext dbContext)
	: IRequestHandler<GetCategoriesQuery, PaginatedResult<CategoryDto>>
{
	public async Task<PaginatedResult<CategoryDto>> Handle(
		GetCategoriesQuery query,
		CancellationToken cancellationToken)
	{
		IQueryable<Category> dbQuery = dbContext.Categories.AsNoTracking().Include(x => x.Recipes);

		if (!string.IsNullOrWhiteSpace(query.Search))
		{
			var keyword = query.Search.Trim().ToLower();

			dbQuery = dbQuery.Where(c => c.Name.Contains(query.Search) || 
			                             (c.Description != null && c.Description.ToLower().Contains(keyword)));
		}

		// Đếm tổng trước khi phân trang (không load dữ liệu)
		var totalCount = await dbQuery.CountAsync(cancellationToken);

		// Sắp xếp
		dbQuery = (query.SortBy?.ToLower(), query.Descending) switch
		{
			("createdat", false) => dbQuery.OrderBy(x => x.CreatedAt),
			("createdat", true) => dbQuery.OrderByDescending(x => x.CreatedAt),
			("recipecount", false) => dbQuery.OrderBy(x => x.Recipes.Count),
			("recipecount", true) => dbQuery.OrderByDescending(x => x.Recipes.Count),
			(_, false) => dbQuery.OrderBy(x => x.Name),
			(_, true) => dbQuery.OrderByDescending(x => x.Name),
		};

		// Áp dụng phân trang
		var categories = await dbQuery
			.Skip((query.PageNumber - 1) * query.PageSize)
			.Take(query.PageSize)
			.Select(x => new CategoryDto(
					x.Id,
					x.Name,
					x.Slug,
					x.Description,
					x.ImageUrl,
					x.Recipes.Count,
					x.CreatedAt))
			.ToListAsync(cancellationToken);

		return PaginatedResult<CategoryDto>.Create(categories, totalCount, query.PageNumber, query.PageSize);
	}
}