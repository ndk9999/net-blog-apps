using TechBlog.Core.Entities;

namespace TechBlog.Core.DTO;

public class PostItem
{
	public int Id { get; set; }

	public string Title { get; set; }

	public string ShortDescription { get; set; }

	public string UrlSlug { get; set; }

	public bool Published { get; set; }

	public DateTime PostedDate { get; set; }

	public DateTime? ModifiedDate { get; set; }

	public string CategoryName { get; set; }

	public List<string> Tags { get; set; }
}