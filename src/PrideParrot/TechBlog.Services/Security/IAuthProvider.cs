namespace TechBlog.Services.Security;

public interface IAuthProvider
{
	string Username { get; }

	bool IsLoggedIn { get; }

	Task<bool> LoginAsync(string username, string password, bool remember);

	Task LogoutAsync();
}