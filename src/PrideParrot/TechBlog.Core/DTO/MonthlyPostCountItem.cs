using System.Globalization;

namespace TechBlog.Core.DTO;

public class MonthlyPostCountItem
{
	public int Year { get; set; }

	public int Month { get; set; }

	public int PostCount { get; set; }

	public string MonthName => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month);
}