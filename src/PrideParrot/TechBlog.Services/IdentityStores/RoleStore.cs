using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TechBlog.Core.Entities;
using TechBlog.Data.Contexts;

namespace TechBlog.Services.IdentityStores;

public class RoleStore : RoleStore<Role, BlogDbContext, int, UserInRole, RoleClaim>
{
	public RoleStore(BlogDbContext context, IdentityErrorDescriber describer = null) 
		: base(context, describer)
	{
	}
}