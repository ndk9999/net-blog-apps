using System.ComponentModel;
using Microsoft.Build.Framework;

namespace TechBlog.Web.Models;

public class UnsubscribeModel
{
	[DisplayName("Your Email")]
	public string Email { get; set; }

	[DisplayName("Please Give Us A Reason")]
	public string Reason { get; set; }

	public string Message { get; set; }
}