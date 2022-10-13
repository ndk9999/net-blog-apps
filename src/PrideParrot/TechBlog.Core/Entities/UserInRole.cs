using Microsoft.AspNetCore.Identity;
using TechBlog.Core.Contracts;

namespace TechBlog.Core.Entities;

public class UserInRole : IdentityUserRole<int>, IEntity
{
	public int Id { get; set; }

	public Role Role { get; set; }

	public Account Account { get; set; }
}