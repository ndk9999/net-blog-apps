using System.ComponentModel.DataAnnotations;

namespace TechBlog.Web.Models;

public class SubscribeFormModel
{
	[Required, EmailAddress]
	public string Email { get; set; }
}