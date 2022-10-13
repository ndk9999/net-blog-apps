using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TechBlog.Core.Contracts;

namespace TechBlog.Web.Models;

public class LoginModel : IRequireCaptcha
{
	[DisplayName("Username (*)")]
	public string UserName { get; set; }

	[DisplayName("Password (*)")]
	public string Password { get; set; }

	[DisplayName("Remember Me")]
	public bool RememberMe { get; set; }

	public string CaptchaToken { get; set; }
}