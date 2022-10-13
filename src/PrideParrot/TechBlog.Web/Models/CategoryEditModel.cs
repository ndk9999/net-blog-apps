using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechBlog.Web.Models;

public class CategoryEditModel
{
	public int Id { get; set; }

	[DisplayName("Category Name")]
	public string Name { get; set; }

	[DisplayName("Slug")]
	public string UrlSlug { get; set; }

	public string Description { get; set; }

	public bool ShowOnMenu { get; set; }
}