using TechBlog.Core.Contracts;

namespace TechBlog.Core.Entities;

public class Subscriber : IEntity
{
	public int Id { get; set; }

	public string Email { get; set; }

	public DateTime SubscribedDate { get; set; }

	public DateTime? UnsubscribedDate { get; set; }

	public string UnsubscribedReason { get; set; }

	public bool Voluntary { get; set; }

	public string Notes { get; set; }
}