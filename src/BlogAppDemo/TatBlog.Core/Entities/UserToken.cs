using Microsoft.AspNetCore.Identity;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities;

public class UserToken : IdentityUserToken<int>, IEntity
{
	public int Id { get; set; }
}