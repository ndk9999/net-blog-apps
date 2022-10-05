using TechBlog.Core.Constants;
using TechBlog.Core.Entities;
using TechBlog.Core.Repositories;

namespace TechBlog.Web.Models;

public class PostListViewModel
{
	public IList<Post> Posts { get; init; }

	public int TotalPosts { get; init; }

	public static async Task<PostListViewModel> CreateAsync(IBlogRepository blogRepository, int pageNumber)
	{
		pageNumber = Math.Max(1, pageNumber);

		return new PostListViewModel
		{
			Posts = await blogRepository.GetPostsAsync(pageNumber, Default.Number.PageSize),
			TotalPosts = await blogRepository.CountPostsAsync()
		};
	}
}