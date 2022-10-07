using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechBlog.Core.Entities;
using TechBlog.Web.Models;
using TechBlog.Web.Providers;

namespace TechBlog.Web.Controllers;

[Authorize]
public class AdminController : Controller
{
	private readonly IAuthProvider _authProvider;

	public AdminController(IAuthProvider authProvider)
	{
		_authProvider = authProvider;
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

	private IActionResult RedirectToUrl(string returnUrl)
	{
		return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
			? Redirect(returnUrl)
			: RedirectToAction("Dashboard");
	}
}