using System.ComponentModel.DataAnnotations;

namespace TechBlog.Web.Models;

public class ContactFormModel
{
	[Required]
	public string FullName { get; set; }

	[Required, EmailAddress]
	public string Email { get; set; }

	[Required]
	public string Subject { get; set; }

	[Required]
	public string Message { get; set; }
}