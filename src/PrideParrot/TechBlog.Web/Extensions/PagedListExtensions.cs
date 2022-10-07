using TechBlog.Core.Contracts;
using TechBlog.Web.Models;

namespace TechBlog.Web.Extensions;

public static class PagedListExtensions
{
	public static GridResponse<T> ToGridResponse<T>(this IPagedList<T> pagedList)
	{
		return new GridResponse<T>()
		{
			Page = pagedList.PageNumber,
			Records = pagedList.TotalItemCount,
			Total = pagedList.PageCount,
			Rows = pagedList
		};
	}
}