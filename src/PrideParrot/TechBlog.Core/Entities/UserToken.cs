using Microsoft.AspNetCore.Identity;
using TechBlog.Core.Contracts;

namespace TechBlog.Core.Entities;

public class UserToken : IdentityUserToken<int>, IEntity
{
	public int Id { get; set; }
}