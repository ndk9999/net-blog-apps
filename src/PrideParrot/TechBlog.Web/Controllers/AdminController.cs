using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechBlog.Core.DTO;
using TechBlog.Core.Entities;
using TechBlog.Core.Repositories;
using TechBlog.Web.Extensions;
using TechBlog.Web.Models;
using TechBlog.Web.Providers;

namespace TechBlog.Web.Controllers;

[Authorize]
public class AdminController : Controller
{
	private readonly IAuthProvider _authProvider;
	private readonly IBlogRepository _blogRepository;
	private readonly IMapper _mapper;

	public AdminController(
		IAuthProvider authProvider, 
		IBlogRepository blogRepository, IMapper mapper)
	{
		_authProvider = authProvider;
		_blogRepository = blogRepository;
		_mapper = mapper;
	}

	// GET
	[HttpGet, AllowAnonymous]
	public IActionResult Login(string returnUrl)
	{
		if (_authProvider.IsLoggedIn)
		{
			return RedirectToUrl(returnUrl);
		}

		ViewBag.ReturnUrl = returnUrl;

		return View();
	}

	[HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
	public async Task<IActionResult> Login(LoginModel model, string returnUrl)
	{
		if (ModelState.IsValid && 
		    await _authProvider.LoginAsync(model.UserName, model.Password, model.RememberMe))
		{
			return RedirectToUrl(returnUrl);
		}

		ModelState.AddModelError("", "Username or password is invalid");
		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> Logout()
	{
		await _authProvider.LogoutAsync();

		return RedirectToAction("Login");
	}

	public IActionResult Dashboard()
	{
		return View();
	}

	public async Task<IActionResult> Posts(PostFilterModel model)
	{
		var categories = await _blogRepository.GetCategoriesAsync();

		model.CategoryList = categories
			.Select(x => new SelectListItem()
			{
				Text = $"{x.Name} ({x.PostCount})",
				Value = x.Id.ToString()
			})
			.ToList();

		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> GridPosts(PostFilterModel filterModel, GridRequestModel gridModel)
	{
		var postQuery = _mapper.Map<PostQuery>(filterModel);
		var postsList = await _blogRepository.GetPagedPostsAsync(
			postQuery, gridModel, posts => posts.ProjectToType<PostItem>());

		return Json(postsList.ToGridResponse());
	}

	[HttpPost]
	public async Task<IActionResult> TogglePostFlag(int id, string flagName)
	{
		if (flagName != "published")
		{
			return Json(AjaxResponse.Error("Invalid flag name"));
		}

		var flagStatus = await _blogRepository.TogglePublishedFlagAsync(id);

		return Json(AjaxResponse.Ok(flagStatus));
	}

	public async Task<IActionResult> EditPost(int? id)
	{
		var post = id > 0
			? await _blogRepository.GetPostByIdAsync(id.Value, true)
			: null;

		var model = post == null
			? new PostEditModel()
			: _mapper.Map<PostEditModel>(post);

		await PopulatePostEditModel(model);

		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> EditPost(PostEditModel model, string nextAction = "Close")
	{
		var post = model.Id > 0 ? await _blogRepository.GetPostByIdAsync(model.Id) : null;

		if (await _blogRepository.IsPostSlugExistedAsync(model.UrlSlug) &&
		    (post == null || post.UrlSlug != model.UrlSlug))
		{
			ModelState.AddModelError("UrlSlug", $"Slug '{model.UrlSlug}' is already in use");
		}

		if (!ModelState.IsValid)
		{
			await PopulatePostEditModel(model);
			return View(model);
		}
		
		if (post == null)
		{
			post = _mapper.Map<Post>(model);

			post.Id = 0;
			post.PostedDate = DateTime.Now;
		}
		else
		{
			_mapper.Map(model, post);

			post.Category = null;
			post.ModifiedDate = DateTime.Now;
		}

		var tags = model.SelectedTags.Split(
			new[] {'\r', '\n', '\t', ',', ';'}, 
			StringSplitOptions.RemoveEmptyEntries);
		
		await _blogRepository.CreateOrUpdatePostAsync(post, tags);

		return nextAction == "Continue"
			? RedirectToAction("EditPost", new {model.Id})
			: RedirectToAction("Posts");
	}

	[HttpPost]
	public async Task<IActionResult> VerifyPostSlug(string urlSlug)
	{
		var slugExisted = await _blogRepository.IsPostSlugExistedAsync(urlSlug);

		return slugExisted
			? Json($"Slug '{urlSlug}' is already in use")
			: Json(true);
	}


	public IActionResult Tags()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> GridTags(GridRequestModel gridModel)
	{
		var pagedTags = await _blogRepository.GetPagedTagsAsync(gridModel);

		return Json(pagedTags.ToGridResponse());
	}

	[HttpPost]
	public async Task<IActionResult> DeleteTag(int id)
	{
		if (await _blogRepository.DeleteTagAsync(id))
		{
			return Json(new
			{
				Id = id,
				Success = true,
				Message = $"Tag '{id}' has been deleted"
			});
		}

		return Json(new
		{
			Id = 0,
			Success = false,
			Message = $"Tag '{id}' not found"
		});
	}



	public IActionResult Categories()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> GridCategories(GridRequestModel gridModel)
	{
		var pagedCategories = await _blogRepository.GetPagedCategoriesAsync(gridModel);

		return Json(pagedCategories.ToGridResponse());
	}

	[HttpPost]
	public async Task<IActionResult> EditCategory(CategoryEditModel model)
	{
		var category = model.Id > 0
			? await _blogRepository.GetCategoryByIdAsync(model.Id)
			: null;

		if (await _blogRepository.IsCategorySlugExistedAsync(model.UrlSlug) &&
		    (category == null || category.UrlSlug != model.UrlSlug))
		{
			ModelState.AddModelError("UrlSlug", $"Slug '{model.UrlSlug}' is already in use");
		}

		if (!ModelState.IsValid)
		{
			return Json(new
			{
				Id = 0,
				Success = false,
				Message = "Invalid category data"
			});
		}

		if (category == null)
		{
			category = _mapper.Map<Category>(model);
			category.Id = 0;
		}
		else
		{
			_mapper.Map(model, category);
		}

		await _blogRepository.CreateOrUpdateCategoryAsync(category);

		return Json(new
		{
			Id = category.Id,
			Success = true,
			Message = $"Category '{model.Name}' has been saved"
		});
	}

	[HttpPost]
	public async Task<IActionResult> DeleteCategory(int id)
	{
		if (await _blogRepository.DeleteCategoryAsync(id))
		{
			return Json(new
			{
				Id = id,
				Success = true,
				Message = $"Category '{id}' has been deleted"
			});
		}

		return Json(new
		{
			Id = 0,
			Success = false,
			Message = $"Category '{id}' not found"
		});
	}



	private async Task PopulatePostEditModel(PostEditModel model)
	{
		var categories = await _blogRepository.GetCategoriesAsync();
		var tags = await _blogRepository.GetTagsAsync();

		model.CategoryList = categories.Select(x => new SelectListItem()
		{
			Value = x.Id.ToString(),
			Text = x.Name,
		}).ToList();

		model.TagList = tags.Select(x => new SelectListItem()
		{
			Value = x.Id.ToString(),
			Text = x.Name,
		}).ToList();
	}

	private IActionResult RedirectToUrl(string returnUrl)
	{
		return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
			? Redirect(returnUrl)
			: RedirectToAction("Dashboard");
	}
}