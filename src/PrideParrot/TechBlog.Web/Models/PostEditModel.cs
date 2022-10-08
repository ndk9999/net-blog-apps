using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechBlog.Web.Models;

public class PostEditModel
{
	public int Id { get; set; }

	[Required, DisplayName("Post Title")]
	[StringLength(500, ErrorMessage = "Title: Length should not exceed 500 characters")]
	public string Title { get; set; }

	[Required, DisplayName("Introduction")]
	public string ShortDescription { get; set; }

	[Required, DisplayName("Main Content")]
	public string Description { get; set; }

	[Required, DisplayName("Metadata")]
	[StringLength(1000, ErrorMessage = "Meta: Length should not exceed 1000 characters")]
	public string Meta { get; set; }

	[Required, DisplayName("Slug")]
	[StringLength(1000, ErrorMessage = "Meta: UrlSlug should not exceed 50 characters")]
	[Remote("VerifyPostSlug", "Admin", HttpMethod = "POST")]
	public string UrlSlug { get; set; }

	[DisplayName("Publish this post")]
	public bool Published { get; set; }

	[Required, DisplayName("Category")]
	public int CategoryId { get; set; }

	[Required, DisplayName("Enter Tags (One tag on a line)")]
	public string SelectedTags { get; set; }

	public IEnumerable<SelectListItem> CategoryList { get; set; }

	public IEnumerable<SelectListItem> TagList { get; set; }
}