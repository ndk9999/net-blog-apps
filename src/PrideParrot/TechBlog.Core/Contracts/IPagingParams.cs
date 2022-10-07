using System.Xml.Linq;

namespace TechBlog.Core.Contracts;

public interface IPagingParams
{
	int PageSize { get; set; }

	/// <summary>
	/// One-based page index
	/// </summary>
	int PageNumber { get; set; }

	string SortColumn { get; set; }

	string SortOrder { get; set; }

	public string GetOrderExpression(string defaultColumn = "Id")
	{
		var column = string.IsNullOrWhiteSpace(SortColumn) ? defaultColumn : SortColumn;
		var order = "ASC".Equals(SortOrder, StringComparison.OrdinalIgnoreCase) ? SortOrder : "DESC";

		return $"{column} {order}";
	}
}