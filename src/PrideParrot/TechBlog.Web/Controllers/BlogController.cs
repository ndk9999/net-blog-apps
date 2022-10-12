using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using TechBlog.Core.Constants;
using TechBlog.Core.DTO;
using TechBlog.Services.Blogs;
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

	// GET /search?s=keyword
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

	// GET /tag/slug?p=10
	public async Task<IActionResult> Archive(int year, int month, int p = 1)
	{
		var postQuery = new PostQuery() { Year = year, Month = month };
		var model = await PostListViewModel.CreateAsync(Default.PostQueryPurpose.SearchByKeyword, _blogRepository, postQuery, p);
		var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

		model.Keyword = $"{monthName} {year}";

		ViewBag.PageTitle = $"List of posts in {monthName} {year}";

		return View("List", model);
	}

	// GET /post/year/month/slug
	public async Task<IActionResult> Post(int year, int month, int day, string slug)
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

		await _blogRepository.IncreaseViewCountAsync(post.Id);

		ViewBag.PageTitle = post.Title;

		return View("Single", post);
	}
}