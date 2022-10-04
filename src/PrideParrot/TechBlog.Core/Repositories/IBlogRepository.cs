using TechBlog.Core.Entities;

namespace TechBlog.Core.Repositories;

public interface IBlogRepository
{
	Task<IList<Post>> GetPosts(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

	Task<int> CountTotalPosts(CancellationToken cancellationToken = default);
}