using Microsoft.AspNetCore.Mvc;
using TechBlog.Core.Repositories;
using TechBlog.Web.Models;

namespace TechBlog.Web.Controllers;

public class BlogController : Controller
{
	private readonly IBlogRepository _blogRepository;

	public BlogController(IBlogRepository blogRepository)
	{
		_blogRepository = blogRepository;
	}

	// GET
	public async Task<IActionResult> Posts(int p = 1)
	{
		var model = await PostListViewModel.CreateAsync(_blogRepository, p);
		
		ViewBag.PageTitle = "Latest Posts";

		return View("List", model);
	}
}