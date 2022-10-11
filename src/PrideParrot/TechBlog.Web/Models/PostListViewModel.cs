using TechBlog.Core.Constants;
using TechBlog.Core.DTO;
using TechBlog.Core.Entities;
using TechBlog.Services.Blogs;

namespace TechBlog.Web.Models;

public class PostListViewModel
{
	public IList<Post> Posts { get; init; }

	public int TotalPosts { get; init; }

	public string Keyword { get; set; }

	public Category Category { get; set; }

	public Tag Tag { get; set; }


	public static async Task<PostListViewModel> CreateAsync(string purpose, IBlogRepository blogRepository, PostQuery condition, int pageNumber)
	{
		pageNumber = Math.Max(1, pageNumber);

		return purpose switch
		{
			Default.PostQueryPurpose.FilterByCategory => new PostListViewModel
			{
				Posts = await blogRepository.GetPostsAsync(condition, pageNumber, Default.Number.PageSize),
				TotalPosts = await blogRepository.CountPostsAsync(condition),
				Category = await blogRepository.GetCategoryAsync(condition.CategorySlug)
			},
			Default.PostQueryPurpose.FilterByTag => new PostListViewModel
			{
				Posts = await blogRepository.GetPostsAsync(condition, pageNumber, Default.Number.PageSize),
				TotalPosts = await blogRepository.CountPostsAsync(condition),
				Tag = await blogRepository.GetTagAsync(condition.TagSlug)
			},
			Default.PostQueryPurpose.SearchByKeyword => new PostListViewModel
			{
				Posts = await blogRepository.GetPostsAsync(condition, pageNumber, Default.Number.PageSize),
				TotalPosts = await blogRepository.CountPostsAsync(condition),
				Keyword = condition.Keyword
			},
			_ => new PostListViewModel
			{
				Posts = await blogRepository.GetPostsAsync(condition, pageNumber, Default.Number.PageSize),
				TotalPosts = await blogRepository.CountPostsAsync(condition)
			}
		};
	}
}