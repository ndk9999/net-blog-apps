using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechBlog.Web.Models;

public class CategoryEditModel
{
	public int Id { get; set; }

	[Required, DisplayName("Category Name"), MaxLength(50)]
	public string Name { get; set; }

	[Required, DisplayName("Slug"), MaxLength(50)]
	public string UrlSlug { get; set; }

	[MaxLength(500)]
	public string Description { get; set; }

	public bool ShowOnMenu { get; set; }
}