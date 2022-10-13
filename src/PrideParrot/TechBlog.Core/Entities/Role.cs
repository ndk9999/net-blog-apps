using Microsoft.AspNetCore.Identity;
using TechBlog.Core.Contracts;

namespace TechBlog.Core.Entities;

public class Role : IdentityRole<int>, IEntity
{
}