using Microsoft.AspNetCore.Identity;

namespace TechBlog.Core.Entities;

public class UserToken : IdentityUserToken<int>
{
	public int Id { get; set; }
}