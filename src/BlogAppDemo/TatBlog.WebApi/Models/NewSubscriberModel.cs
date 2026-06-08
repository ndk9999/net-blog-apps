using System.ComponentModel.DataAnnotations;

namespace TatBlog.WebApi.Models;

public class NewSubscriberModel
{
	[Required]
	public string Email { get; set; }
}