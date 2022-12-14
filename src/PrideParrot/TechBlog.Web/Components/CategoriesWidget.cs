using Microsoft.AspNetCore.Mvc;
using TechBlog.Services.Blogs;

namespace TechBlog.Web.Components;

public class CategoriesWidget : ViewComponent
{
	private readonly IBlogRepository _blogRepository;

	public CategoriesWidget(IBlogRepository blogRepository)
	{
		_blogRepository = blogRepository;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var categories = await _blogRepository.GetCategoriesAsync();

		categories = categories.Where(x => !x.ShowOnMenu).ToList();

		return View(categories);
	}
}