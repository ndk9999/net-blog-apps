using System.ComponentModel;

namespace TechBlog.Web.Models;

public class BlockSubscriberModel
{
	public int Id { get; set; }

	public string Email { get; set; }

	[DisplayName("Reason")]
	public string BlockedReason { get; set; }

	public bool Involuntary { get; set; }

	public string Notes { get; set; }
}