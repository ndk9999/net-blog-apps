using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechBlog.Core.Constants;
using TechBlog.Core.DTO;
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

	// GET /?p=10
	// GET /posts?p=10
	public async Task<IActionResult> Posts(int p = 1)
	{
		var postQuery = new PostQuery();
		var model = await PostListViewModel.CreateAsync(Default.PostQueryPurpose.LatestPosts, _blogRepository, postQuery, p);
		
		ViewBag.PageTitle = "Latest Posts";

		return View("List", model);
	}

	// GET /category/slug?p=10
	public async Task<IActionResult> Category(string slug, int p = 1)
	{
		var postQuery = new PostQuery() {CategorySlug = slug};
		var model = await PostListViewModel.CreateAsync(Default.PostQueryPurpose.FilterByCategory, _blogRepository, postQuery, p);

		if (model.Category == null)
		{
			return View("Error", new ErrorViewModel
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				Message = $"Category '{slug}' Not Found"
			});
		}

		ViewBag.PageTitle = $"Latest posts on category '{model.Category.Name}'";

		return View("List", model);
	}

	// GET /tag/slug?p=10
	public async Task<IActionResult> Tag(string slug, int p = 1)
	{
		var postQuery = new PostQuery() { TagSlug = slug };
		var model = await PostListViewModel.CreateAsync(Default.PostQueryPurpose.FilterByTag, _blogRepository, postQuery, p);

		if (model.Tag == null)
		{
			return View("Error", new ErrorViewModel
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				Message = $"Tag '{slug}' Not Found"
			});
		}

		ViewBag.PageTitle = $"Latest posts on tag '{model.Tag.Name}'";

		return View("List", model);
	}

	// GET /tag/slug?p=10
	public async Task<IActionResult> Search(string s, int p = 1)
	{
		if (string.IsNullOrWhiteSpace(s))
		{
			return RedirectToAction("Posts");
		}

		var postQuery = new PostQuery() { Keyword = s };
		var model = await PostListViewModel.CreateAsync(Default.PostQueryPurpose.SearchByKeyword, _blogRepository, postQuery, p);
		
		ViewBag.PageTitle = $"List of posts found for search text '{s}'";

		return View("List", model);
	}

	// GET /post/year/month/slug
	public async Task<IActionResult> Post(int year, int month, string slug)
	{
		var post = await _blogRepository.GetPostAsync(year, month, slug);

		if (post == null)
		{
			return View("Error", new ErrorViewModel
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				Message = $"Post '{slug}' Not Found"
			});
		}

		// TODO: Check if the post is published or not. Admin can see all posts that are not published.
		if (!post.Published)
		{
			return View("Error", new ErrorViewModel
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				Message = "The post is not published yet"
			});
		}

		ViewBag.PageTitle = post.Title;

		return View("Single", post);
	}
}