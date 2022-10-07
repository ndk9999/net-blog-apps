namespace TechBlog.Web.Models;

public class GridResponse<T>
{
	/// <summary>
	/// Current page number
	/// </summary>
	public int Page { get; set; }

	/// <summary>
	/// Total number of pages
	/// </summary>
	public int Total { get; set; }

	/// <summary>
	/// Total number of records
	/// </summary>
	public int Records { get; set; }

	/// <summary>
	/// Set of records
	/// </summary>
	public IEnumerable<T> Rows { get; set; }

}