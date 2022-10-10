using System.ComponentModel.DataAnnotations;
using TechBlog.Core.Contracts;

namespace TechBlog.Web.Models;

public class ContactFormModel : IRequireCaptcha
{
	[Required]
	public string FullName { get; set; }

	[Required, EmailAddress]
	public string Email { get; set; }

	[Required]
	public string Subject { get; set; }

	[Required]
	public string Message { get; set; }

	[Required]
	public string CaptchaToken { get; set; }
}