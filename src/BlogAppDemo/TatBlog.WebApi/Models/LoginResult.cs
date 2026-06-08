namespace TatBlog.WebApi.Models;

public record LoginResult(
	int Id,
	string Username,
	string Email,
	string FullName,
	string Token);