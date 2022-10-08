namespace TechBlog.Core.DTO;

public class PostQuery
{
	public bool PublishedOnly { get; set; }

	public bool NotPublished { get; set; }

	public int? CategoryId { get; set; }

	public string CategorySlug { get; set; }

	public string TagSlug { get; set; }

	public string Keyword { get; set; }

	public int? Year { get; set; }

	public int? Month { get; set; }

	public string TitleSlug { get; set; }


	public PostQuery()
	{
		PublishedOnly = true;
	}
}