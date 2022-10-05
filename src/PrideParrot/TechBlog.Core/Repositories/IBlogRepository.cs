using TechBlog.Core.Entities;

namespace TechBlog.Core.Repositories;

public interface IBlogRepository
{
	Task<IList<Post>> GetPostsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

	Task<int> CountPostsAsync(CancellationToken cancellationToken = default);
}