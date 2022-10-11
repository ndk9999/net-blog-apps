using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TechBlog.Core.Entities;
using TechBlog.Data.Contexts;

namespace TechBlog.Services.IdentityStores;

public class AccountStore : UserStore<
	Account, 
	Role, 
	BlogDbContext, 
	int, 
	UserClaim, 
	UserInRole, 
	UserLogin, 
	UserToken, 
	RoleClaim>
{
	public AccountStore(BlogDbContext context, IdentityErrorDescriber describer = null) 
		: base(context, describer)
	{
	}

	public override Task SetPasswordHashAsync(
		Account user, 
		string passwordHash,
		CancellationToken cancellationToken = new CancellationToken())
	{
		if (!string.IsNullOrWhiteSpace(passwordHash) && passwordHash != user.PasswordHash)
		{
			user.PwdChangedDate = DateTime.Now;
		}

		return base.SetPasswordHashAsync(user, passwordHash, cancellationToken);	
	}
}