using Microsoft.AspNetCore.Identity;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities;

public class UserLogin : IdentityUserLogin<int>, IEntity
{
	public int Id { get; set; }
}