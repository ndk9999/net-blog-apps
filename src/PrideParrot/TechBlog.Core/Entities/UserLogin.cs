using Microsoft.AspNetCore.Identity;
using TechBlog.Core.Contracts;

namespace TechBlog.Core.Entities;

public class UserLogin : IdentityUserLogin<int>, IEntity
{
	public int Id { get; set; }
}