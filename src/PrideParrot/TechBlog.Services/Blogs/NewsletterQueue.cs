using System.Collections.Concurrent;
using TechBlog.Core.Entities;

namespace TechBlog.Services.Blogs;

public class NewsletterQueue : INewsletterQueue
{
	private readonly ConcurrentQueue<Post> _queue;

	public NewsletterQueue()
	{
		_queue = new ConcurrentQueue<Post>();
	}

	public void Enqueue(Post post)
	{
		_queue.Enqueue(post);
	}

	public bool TryDequeue(out Post post)
	{
		return _queue.TryDequeue(out post);
	}
}