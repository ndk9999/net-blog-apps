using Microsoft.AspNetCore.Mvc;
using TechBlog.Services.Blogs;

namespace TechBlog.Web.Components;

public class ArchivesWidget : ViewComponent
{
	private readonly IBlogRepository _blogRepository;

	public ArchivesWidget(IBlogRepository blogRepository)
	{
		_blogRepository = blogRepository;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var counters = await _blogRepository.CountMonthlyPostsAsync(18);

		return View(counters);
	}
}