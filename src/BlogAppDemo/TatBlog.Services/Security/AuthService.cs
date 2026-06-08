using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Security;

public class AuthService : IAuthService
{
	private const string GUEST_USERNAME = "Guest";
	private readonly SignInManager<Account> _signInManager;
	private readonly IHttpContextAccessor _contextAccessor;

	public AuthService(
		SignInManager<Account> signInManager,
		IHttpContextAccessor contextAccessor)
	{
		_signInManager = signInManager;
		_contextAccessor = contextAccessor;
	}

	public string Username => _contextAccessor.HttpContext?.User.Identity?.Name ?? GUEST_USERNAME;

	public bool IsLoggedIn => _contextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

	public async Task<Account> GetLoggedInUserAsync(string username)
	{
		return await _signInManager.UserManager.FindByNameAsync(username);
	}

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