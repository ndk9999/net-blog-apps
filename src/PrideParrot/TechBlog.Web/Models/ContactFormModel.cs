using System.ComponentModel.DataAnnotations;
using TechBlog.Core.Contracts;

namespace TechBlog.Web.Models;

public class ContactFormModel : IRequireCaptcha
{
	public string FullName { get; set; }

	public string Email { get; set; }

	public string Subject { get; set; }

	public string Message { get; set; }

	public string CaptchaToken { get; set; }
}