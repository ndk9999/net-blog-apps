using Microsoft.AspNetCore.Identity;
using TechBlog.Core.Contracts;

namespace TechBlog.Core.Entities;

public class Account : IdentityUser<int>, IEntity
{
	public string FullName { get; set; }

	public DateTime PwdChangedDate { get; set; }
}