using Microsoft.AspNetCore.Identity;

namespace TechBlog.Core.Entities;

public class UserLogin : IdentityUserLogin<int>
{
	public int Id { get; set; }
}