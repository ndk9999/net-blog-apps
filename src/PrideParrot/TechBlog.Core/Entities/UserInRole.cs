using Microsoft.AspNetCore.Identity;

namespace TechBlog.Core.Entities;

public class UserInRole : IdentityUserRole<int>
{
	public int Id { get; set; }

	public Role Role { get; set; }

	public Account Account { get; set; }
}