using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechBlog.Core.Entities;
using TechBlog.Web.Models;

namespace TechBlog.Web.Controllers;

[Authorize]
public class AdminController : Controller
{
	private readonly SignInManager<Account> _signInManager;

	public AdminController(SignInManager<Account> signInManager)
	{
		_signInManager = signInManager;
	}

	// GET
	[HttpGet, AllowAnonymous]
	public IActionResult Login(string returnUrl)
	{
		if (User.Identity.IsAuthenticated)
		{
			return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
				? Redirect(returnUrl)
				: RedirectToAction("Manage");
		}

		ViewBag.ReturnUrl = returnUrl;

		return View();
	}

	[HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
	public async Task<IActionResult> Login(LoginModel model, string returnUrl)
	{
		if (!ModelState.IsValid)
		{
			return View(model);
		}

		var loginResult = await _signInManager.PasswordSignInAsync(
			model.UserName, 
			model.Password, 
			model.RememberMe, 
			true);

		if (loginResult.Succeeded)
		{
			return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
				? Redirect(returnUrl)
				: RedirectToAction("Manage");
		}

		ModelState.AddModelError("", "Username or password is invalid");
		return View(model);
	}

	public IActionResult Manage()
	{
		return View();
	}
}