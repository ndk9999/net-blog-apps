using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SlugGenerator;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers;

public class PostsController : Controller
{
	private readonly IBlogRepository _blogRepository;
	private readonly IAuthorRepository _authorRepository;
	private readonly IMediaManager _mediaManager;

	public PostsController(IBlogRepository blogRepository, IAuthorRepository authorRepository, IMediaManager mediaManager)
	{
		_blogRepository = blogRepository;
		_authorRepository = authorRepository;
		_mediaManager = mediaManager;
	}

	public async Task<IActionResult> Index(PostFilterModel model)
	{
		var postQuery = new PostQuery()
		{
			Keyword = model.Keyword,
			AuthorId = model.AuthorId,
			CategoryId = model.CategoryId,
			Year = model.Year,
			Month = model.Month
		};

		var postsList = await _blogRepository
			.GetPagedPostsAsync(postQuery, 1, 10);

		await PopulatePostFilterModelAsync(model);
		ViewBag.PostsList = postsList;

		return View(model);
	}

	[HttpGet]
	public async Task<IActionResult> Edit(int? id)
	{
		var post = id > 0 ? await _blogRepository.GetPostByIdAsync(id.Value) : null;
		var model = new PostEditModel();

		if (post != null)
		{
			model.Id = post.Id;
			model.AuthorId = post.AuthorId;
			model.CategoryId = post.CategoryId;
			model.Title = post.Title;
			model.ShortDescription = post.ShortDescription;
			model.Description = post.Description;
			model.Meta = post.Meta;
			model.ImageUrl = post.ImageUrl;
			model.Published = post.Published;

			if (post.Tags != null)
				model.SelectedTags = string.Join("\r\n", post.Tags.Select(t => t.Name));

		}

		await PopulatePostEditModelAsync(model);
		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> Edit(PostEditModel model)
	{
		if (!string.IsNullOrWhiteSpace(model.Title))
		{
			var slug = model.Title.GenerateSlug();
			var existed = await _blogRepository.IsPostSlugExistedAsync(model.Id, slug);

			if (existed)
			{
				ModelState.AddModelError("", "Slug đã dùng cho một bài viết khác");
			}
		}

		if (!ModelState.IsValid)
		{
			await PopulatePostEditModelAsync(model);
			return View(model);
		}

		// Lấy bài viết cũ từ database
		var post = model.Id > 0 ? await _blogRepository.GetPostByIdAsync(model.Id) : null;

		if (post == null)
		{
			post = new Post()
			{
				PostedDate = DateTime.Now
			};
		}

		post.Title = model.Title;
		post.AuthorId = model.AuthorId;
		post.CategoryId = model.CategoryId;
		post.ShortDescription = model.ShortDescription;
		post.Description = model.Description;
		post.Meta = model.Meta;
		post.Published = model.Published;
		post.ModifiedDate = DateTime.Now;
		post.UrlSlug = model.Title.GenerateSlug();

		if (model.ImageFile?.Length > 0)
		{
			var uploadedPath = await _mediaManager.SaveFileAsync(
				model.ImageFile.OpenReadStream(),
				model.ImageFile.FileName,
				model.ImageFile.ContentType);

			if (!string.IsNullOrWhiteSpace(uploadedPath))
			{
				post.ImageUrl = uploadedPath;
			}
		}

		await _blogRepository.CreateOrUpdatePostAsync(
			post, model.GetSelectedTags());

		return RedirectToAction(nameof(Index));
	}

	private async Task PopulatePostFilterModelAsync(PostFilterModel model)
	{
		var authors = await _authorRepository.GetAuthorsAsync();
		var categories = await _blogRepository.GetCategoriesAsync();

		model.AuthorList = authors.Select(a => new SelectListItem()
		{
			Text = a.FullName,
			Value = a.Id.ToString()
		});

		model.CategoryList = categories.Select(c => new SelectListItem()
		{
			Text = c.Name,
			Value = c.Id.ToString()
		});
	}

	private async Task PopulatePostEditModelAsync(PostEditModel model)
	{
		var authors = await _authorRepository.GetAuthorsAsync();
		var categories = await _blogRepository.GetCategoriesAsync();

		model.AuthorList = authors.Select(a => new SelectListItem()
		{
			Text = a.FullName,
			Value = a.Id.ToString()
		});

		model.CategoryList = categories.Select(c => new SelectListItem()
		{
			Text = c.Name,
			Value = c.Id.ToString()
		});
	}
}