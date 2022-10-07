using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TechBlog.Core.Contexts;
using TechBlog.Core.Entities;

namespace TechBlog.Core.IdentityStores;

public class RoleStore : RoleStore<Role, BlogDbContext, int, UserInRole, RoleClaim>
{
	public RoleStore(BlogDbContext context, IdentityErrorDescriber describer = null) 
		: base(context, describer)
	{
	}
}