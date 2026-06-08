using System.Linq.Dynamic.Core;
using Microsoft.Data.SqlClient;
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

	public static async Task<PagedList<T>> ToPagedListAsync<T>(
		this IQueryable<T> source,
		int pageNumber,
		int pageSize,
		string sortColumn = "Id",
		string sortOrder = "DESC",
		CancellationToken cancellationToken = default)
	{
		var totalCount = await source.CountAsync(cancellationToken);
		var items = await source
			.OrderBy($"{sortColumn} {sortOrder}")
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

		return new PagedList<T>(items, pageNumber, pageSize, totalCount);
	}

	public static string GetOrderExpression(
		this IPagingParams pagingParams, 
		string defaultColumn = "Id")
	{
		var column = string.IsNullOrWhiteSpace(pagingParams.SortColumn) 
			? defaultColumn 
			: pagingParams.SortColumn;

		var order = "ASC".Equals(pagingParams.SortOrder, StringComparison.OrdinalIgnoreCase) 
			? pagingParams.SortOrder 
			: "DESC";

		return $"{column} {order}";
	}
}