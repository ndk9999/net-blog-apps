using Microsoft.EntityFrameworkCore;
using TechBlog.Core.Contexts;
using TechBlog.Core.DTO;
using TechBlog.Core.Entities;

namespace TechBlog.Core.Repositories;

public class BlogRepository : IBlogRepository
{
	private readonly BlogDbContext _context;

	public BlogRepository(BlogDbContext context)
	{
		_context = context;
	}

	public async Task<IList<Post>> GetPostsAsync(PostQuery condition, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
	{
		return await FilterPosts(condition)
			.OrderByDescending(x => x.PostedDate)
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync(cancellationToken: cancellationToken);
	}

	public async Task<int> CountPostsAsync(PostQuery condition, CancellationToken cancellationToken = default)
	{
		return await FilterPosts(condition).CountAsync(cancellationToken: cancellationToken);
	}

	public async Task<Category> GetCategoryAsync(string slug, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Category>()
			.FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
	}

	public async Task<Tag> GetTagAsync(string slug, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Tag>()
			.FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
	}

	public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
	{
		var postQuery = new PostQuery()
		{
			PublishedOnly = false,
			Year = year,
			Month = month,
			TitleSlug = slug
		};

		return await FilterPosts(postQuery).FirstOrDefaultAsync(cancellationToken);
	}

	private IQueryable<Post> FilterPosts(PostQuery condition)
	{
		IQueryable<Post> posts = _context.Set<Post>()
			.Include(x => x.Category)
			.Include(x => x.Tags);

		if (condition.PublishedOnly)
		{
			posts = posts.Where(x => x.Published);
		}

		if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
		{
			posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
		}

		if (!string.IsNullOrWhiteSpace(condition.TagSlug))
		{
			posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
		}

		if (!string.IsNullOrWhiteSpace(condition.Keyword))
		{
			posts = posts.Where(x => x.Title.Contains(condition.Keyword) ||
			                         x.ShortDescription.Contains(condition.Keyword) ||
			                         x.Description.Contains(condition.Keyword) ||
			                         x.Category.Name.Contains(condition.Keyword) ||
			                         x.Tags.Any(t => t.Name.Contains(condition.Keyword)));
		}

		if (condition.Year > 0)
		{
			posts = posts.Where(x => x.PostedDate.Year == condition.Year);
		}

		if (condition.Month > 0)
		{
			posts = posts.Where(x => x.PostedDate.Month == condition.Month);
		}

		if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
		{
			posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
		}

		return posts;
	}
}