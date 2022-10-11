using Microsoft.AspNetCore.Http;
using TechBlog.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace TechBlog.Services.Security;

public class AuthProvider : IAuthProvider
{
	private readonly SignInManager<Account> _signInManager;
	private readonly IHttpContextAccessor _contextAccessor;

	public AuthProvider(
		SignInManager<Account> signInManager, 
		IHttpContextAccessor contextAccessor)
	{
		_signInManager = signInManager;
		_contextAccessor = contextAccessor;
	}

	public string Username => _contextAccessor.HttpContext?.User.Identity?.Name ?? "Anonymous";

	public bool IsLoggedIn => _contextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

	public async Task<bool> LoginAsync(string username, string password, bool remember)
	{
		var loginResult = await _signInManager.PasswordSignInAsync(
			username, password, remember, true);

		return loginResult.Succeeded;
	}

	public async Task LogoutAsync()
	{
		await _signInManager.SignOutAsync();
	}
}