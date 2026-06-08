using Microsoft.AspNetCore.Identity;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities;

public class UserClaim : IdentityUserClaim<int>, IEntity
{
}