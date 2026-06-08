using Microsoft.AspNetCore.Identity;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities;

public class Account : IdentityUser<int>, IEntity
{
	public string FullName { get; set; }

	public DateTime PwdChangedDate { get; set; }
}