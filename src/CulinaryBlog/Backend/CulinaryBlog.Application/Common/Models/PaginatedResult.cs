namespace CulinaryBlog.Application.Common.Models;

/// <summary>
/// Generic wrapper cho kết quả phân trang.
/// Được dùng cho tất cả list endpoints trong Culinary Blog API.
/// </summary>
public class PaginatedResult<T>
{
	public IReadOnlyList<T> Items { get; init; } = [];

	public int TotalCount { get; init; }

	public int PageNumber { get; init; }

	public int PageSize { get; init; }

	public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

	public bool HasPrevPage => PageNumber > 1;

	public bool HasNextPage => PageNumber < TotalPages;


	public static PaginatedResult<T> Create(
		IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
	{
		return new PaginatedResult<T>
		{
			Items = items,
			TotalCount = totalCount,
			PageNumber = pageNumber,
			PageSize = pageSize
		};
	}
}