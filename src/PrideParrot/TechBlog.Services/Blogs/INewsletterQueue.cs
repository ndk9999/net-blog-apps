using TechBlog.Core.Entities;

namespace TechBlog.Services.Blogs;

public interface INewsletterQueue
{
	void Enqueue(Post post);

	bool TryDequeue(out Post post);
}