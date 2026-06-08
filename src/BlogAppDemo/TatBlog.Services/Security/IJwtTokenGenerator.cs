using TatBlog.Core.Entities;

namespace TatBlog.Services.Security;

public interface IJwtTokenGenerator
{
	string GenerateToken(string username);
}