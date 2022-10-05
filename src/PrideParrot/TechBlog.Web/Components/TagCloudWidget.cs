using Microsoft.AspNetCore.Mvc;
using TechBlog.Core.Repositories;

namespace TechBlog.Web.Components;

public class TagCloudWidget : ViewComponent
{
	private readonly IBlogRepository _blogRepository;

	public TagCloudWidget(IBlogRepository blogRepository)
	{
		_blogRepository = blogRepository;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var tagsList = await _blogRepository.GetTagsAsync();

		return View(tagsList);
	}
}