using TatBlog.Core.Entities;

namespace TatBlog.Services.Security;

public interface IAuthService
{
	string Username { get; }

	bool IsLoggedIn { get; }

	Task<Account> GetLoggedInUserAsync(string username);

	Task<bool> LoginAsync(string username, string password, bool remember);

	Task LogoutAsync();

}