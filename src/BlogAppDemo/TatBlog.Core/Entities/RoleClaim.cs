using Microsoft.AspNetCore.Identity;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities;

public class RoleClaim : IdentityRoleClaim<int>, IEntity
{
}