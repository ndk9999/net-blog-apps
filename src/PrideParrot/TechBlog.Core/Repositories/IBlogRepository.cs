using TechBlog.Core.DTO;
using TechBlog.Core.Entities;

namespace TechBlog.Core.Repositories;

public interface IBlogRepository
{
	Task<IList<Post>> GetPostsAsync(PostQuery condition, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

	Task<int> CountPostsAsync(PostQuery condition, CancellationToken cancellationToken = default);
	
	Task<Category> GetCategoryAsync(string slug, CancellationToken cancellationToken = default);

	Task<IList<CategoryItem>> GetCategoriesAsync(CancellationToken cancellationToken = default);

	Task<Tag> GetTagAsync(string slug, CancellationToken cancellationToken = default);

	Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default);
}