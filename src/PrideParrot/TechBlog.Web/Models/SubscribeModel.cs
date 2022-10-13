using System.ComponentModel.DataAnnotations;

namespace TechBlog.Web.Models;

public class SubscribeModel
{
	[Required, EmailAddress]
	public string Email { get; set; }
}