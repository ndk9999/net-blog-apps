using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

	public IActionResult Posts()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> GridPosts(GridRequestModel model)
	{
		var postQuery = new PostQuery();
		var postsList = await _blogRepository.GetPagedPostsAsync(
			postQuery, model, posts => posts.ProjectToType<PostItem>());

		return Json(postsList.ToGridResponse());
	}

	private IActionResult RedirectToUrl(string returnUrl)
	{
		return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
			? Redirect(returnUrl)
			: RedirectToAction("Dashboard");
	}
}