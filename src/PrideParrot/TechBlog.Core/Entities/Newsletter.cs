using TechBlog.Core.Constants;
using TechBlog.Core.Contracts;

namespace TechBlog.Core.Entities;

public class Newsletter : IEntity
{
	public int Id { get; set; }

	public int PostId { get; set; }

	public int SubscriberId { get; set; }

	public NewsletterStatus Status { get; set; }

	public string Notes { get; set; }


	public Post Post { get; set; }

	public Subscriber Subscriber { get; set; }
}