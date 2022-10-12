using Microsoft.AspNetCore.Mvc;
using TechBlog.Services.Blogs;

namespace TechBlog.Web.Components;

public class MenuItemsWidget : ViewComponent
{
	private readonly IBlogRepository _blogRepository;

	public MenuItemsWidget(IBlogRepository blogRepository)
	{
		_blogRepository = blogRepository;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var categories = await _blogRepository.GetCategoriesAsync(true);

		return View(categories);
	}
}