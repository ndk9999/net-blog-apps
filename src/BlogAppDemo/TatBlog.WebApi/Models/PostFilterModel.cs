namespace TatBlog.WebApi.Models;

public class PostFilterModel : PagingModel
{
	public string Keyword { get; set; }

	public string TagSlug { get; set; }

	public string AuthorSlug { get; set; }

	public string CategorySlug { get; set; }

	public int? Year { get; set; }

	public int? Month { get; set; }
}