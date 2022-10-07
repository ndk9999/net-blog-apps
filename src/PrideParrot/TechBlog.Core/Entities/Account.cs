using Microsoft.AspNetCore.Identity;

namespace TechBlog.Core.Entities;

public class Account : IdentityUser<int>
{
	public string FullName { get; set; }

	public DateTime PwdChangedDate { get; set; }
}