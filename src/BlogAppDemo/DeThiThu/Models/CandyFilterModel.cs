using Microsoft.AspNetCore.Mvc;

namespace DeThiThu.Models;

public class CandyFilterModel
{
	public string Name { get; set; }

	public decimal? MinPrice { get; set; }

	public decimal? MaxPrice { get; set;}

	public int? CategoryId { get; set; }

	public string CategoryName { get; set; }
}