using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechBlog.Web.Models;

public class LoginModel
{
	[Required(ErrorMessage = "Username is required")]
	[DisplayName("Username (*)")]
	public string UserName { get; set; }

	[Required(ErrorMessage = "Password is required")]
	[DisplayName("Password (*)")]
	public string Password { get; set; }

	[DisplayName("Remember Me")]
	public bool RememberMe { get; set; }
}