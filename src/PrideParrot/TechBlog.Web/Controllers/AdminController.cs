using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
		if (!ModelState.IsValid)
		{
			await PopulatePostEditModel(model);
			return View(model);
		}

		var post = await _blogRepository.GetPostByIdAsync(model.Id);

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