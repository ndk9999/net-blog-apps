using System.ComponentModel;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechBlog.Web.Models;

public class PostFilterModel
{
	[DisplayName("Keyword")]
	public string Keyword { get; set; }

	[DisplayName("Category")]
	public int? CategoryId { get; set; }

	[DisplayName("Display posts that are not published only")]
	public bool NotPublished { get; set; }

	[DisplayName("Year")]
	public int? Year { get; set; }

	[DisplayName("Month")]
	public int? Month { get; set; }

	public IEnumerable<SelectListItem> CategoryList { get; set; }

	public IEnumerable<SelectListItem> MonthList { get; set; }


	public PostFilterModel()
	{
		MonthList = Enumerable
			.Range(1, 12)
			.Select(x => new SelectListItem()
			{
				Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x),
				Value = x.ToString()
			})
			.ToList();
	}
}