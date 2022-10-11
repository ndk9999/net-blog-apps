using Microsoft.AspNetCore.Mvc;
using TechBlog.Services.Blogs;

namespace TechBlog.Web.Components;

public class PopularArticles : ViewComponent
{
	private readonly IBlogRepository _blogRepository;

	public PopularArticles(IBlogRepository blogRepository)
	{
		_blogRepository = blogRepository;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var posts = await _blogRepository.GetPopularArticlesAsync(3);

		return View(posts);
	}
}