using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechBlog.Web.Models;

public class PostEditModel
{
	public int Id { get; set; }

	[DisplayName("Post Title")]
	public string Title { get; set; }

	[DisplayName("Introduction")]
	public string ShortDescription { get; set; }

	[DisplayName("Main Content")]
	public string Description { get; set; }

	[DisplayName("Metadata")]
	public string Meta { get; set; }

	[DisplayName("Slug")]
	[Remote("VerifyPostSlug", "Admin", HttpMethod = "POST", AdditionalFields = "Id")]
	public string UrlSlug { get; set; }

	[DisplayName("Select Image")]
	public IFormFile ImageFile { get; set; }

	public string ImageUrl { get; set; }

	[DisplayName("Publish this post")]
	public bool Published { get; set; }

	[DisplayName("Category")]
	public int CategoryId { get; set; }

	[DisplayName("Enter Tags (One tag on a line)")]
	public string SelectedTags { get; set; }

	public IEnumerable<SelectListItem> CategoryList { get; set; }

	public IEnumerable<SelectListItem> TagList { get; set; }


	public List<string> GetSelectedTags()
	{
		return (SelectedTags ?? "")
			.Split(new[] { '\r', '\n', '\t', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
			.ToList();
	}
}