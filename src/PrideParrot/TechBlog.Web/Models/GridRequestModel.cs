using Microsoft.AspNetCore.Mvc;
using TechBlog.Core.Contracts;

namespace TechBlog.Web.Models;

public class GridRequestModel : IPagingParams
{
	[BindProperty(Name = "rows")]
	public int PageSize { get; set; }

	[BindProperty(Name = "page")]
	public int PageNumber { get; set; }

	[BindProperty(Name = "sidx")]
	public string SortColumn { get; set; }

	[BindProperty(Name = "sord")]
	public string SortOrder { get; set; }
}