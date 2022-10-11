using TechBlog.Core.Contracts;
using TechBlog.Core.DTO;
using TechBlog.Core.Entities;

namespace TechBlog.Services.Blogs;

public interface IBlogRepository
{
	Task<IList<Post>> GetPostsAsync(
		PostQuery condition, 
		int pageNumber, 
		int pageSize, 
		CancellationToken cancellationToken = default);

	Task<int> CountPostsAsync(
		PostQuery condition, 
		CancellationToken cancellationToken = default);

	Task<IList<MonthlyPostCountItem>> CountMonthlyPostsAsync(
		int numMonths, 
		CancellationToken cancellationToken = default);

	Task<Category> GetCategoryAsync(
		string slug, 
		CancellationToken cancellationToken = default);

	Task<Category> GetCategoryByIdAsync(int categoryId);

	Task<IList<CategoryItem>> GetCategoriesAsync(
		CancellationToken cancellationToken = default);

	Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
		IPagingParams pagingParams,
		CancellationToken cancellationToken = default);

	Task<Category> CreateOrUpdateCategoryAsync(
		Category category, 
		CancellationToken cancellationToken = default);

	Task<bool> IsCategorySlugExistedAsync(
		string categorySlug,
		CancellationToken cancellationToken = default);

	Task<bool> DeleteCategoryAsync(
		int categoryId, 
		CancellationToken cancellationToken = default);


	Task<Tag> GetTagAsync(
		string slug, 
		CancellationToken cancellationToken = default);

	Task<IList<TagItem>> GetTagsAsync(
		CancellationToken cancellationToken = default);

	Task<IPagedList<TagItem>> GetPagedTagsAsync(
		IPagingParams pagingParams,
		CancellationToken cancellationToken = default);

	Task<bool> DeleteTagAsync(
		int tagId, 
		CancellationToken cancellationToken = default);


	Task<Post> GetPostAsync(
		int year, 
		int month, 
		string slug, 
		CancellationToken cancellationToken = default);

	Task<Post> GetPostByIdAsync(
		int postId,
		bool includeDetails = false,
		CancellationToken cancellationToken = default);

	Task<bool> TogglePublishedFlagAsync(
		int postId,
		CancellationToken cancellationToken = default);

	Task<IList<Post>> GetPopularArticlesAsync(
		int numPosts, 
		CancellationToken cancellationToken = default);

	Task<IPagedList<T>> GetPagedPostsAsync<T>(
		PostQuery condition, 
		IPagingParams pagingParams, 
		Func<IQueryable<Post>, IQueryable<T>> mapper);

	Task<Post> CreateOrUpdatePostAsync(
		Post post,
		IEnumerable<string> tags,
		CancellationToken cancellationToken = default);

	Task<bool> IsPostSlugExistedAsync(
		string slug, 
		CancellationToken cancellationToken = default);
}