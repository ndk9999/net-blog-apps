using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TechBlog.Core.Contracts;

namespace TechBlog.Web.Models;

public class LoginModel : IRequireCaptcha
{
	[Required(ErrorMessage = "Username is required")]
	[DisplayName("Username (*)")]
	public string UserName { get; set; }

	[Required(ErrorMessage = "Password is required")]
	[DisplayName("Password (*)")]
	public string Password { get; set; }

	[DisplayName("Remember Me")]
	public bool RememberMe { get; set; }

	[Required]
	public string CaptchaToken { get; set; }
}