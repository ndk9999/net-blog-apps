using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using TechBlog.Core.Collections;
using TechBlog.Core.Contracts;

namespace TechBlog.Services.Extensions;

public static class PagedListExtensions
{
	public static async Task<PagedList<T>> ToPagedListAsync<T>(
		this IQueryable<T> source,
		IPagingParams pagingParams,
		CancellationToken cancellationToken = default)
	{
		var totalCount = await source.CountAsync(cancellationToken);
		var items = await source
			.OrderBy(pagingParams.GetOrderExpression())
			.Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
			.Take(pagingParams.PageSize)
			.ToListAsync(cancellationToken);

		return new PagedList<T>(items, pagingParams.PageNumber, pagingParams.PageSize, totalCount);
	}
}