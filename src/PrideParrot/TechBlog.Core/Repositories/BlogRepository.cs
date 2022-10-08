using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using TechBlog.Core.Collections;
using TechBlog.Core.Contexts;
using TechBlog.Core.Contracts;
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

	public async Task<IList<Post>> GetPostsAsync(
		PostQuery condition, 
		int pageNumber, 
		int pageSize, 
		CancellationToken cancellationToken = default)
	{
		return await FilterPosts(condition)
			.OrderByDescending(x => x.PostedDate)
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync(cancellationToken: cancellationToken);
	}

	public async Task<int> CountPostsAsync(
		PostQuery condition, CancellationToken cancellationToken = default)
	{
		return await FilterPosts(condition).CountAsync(cancellationToken: cancellationToken);
	}

	public async Task<IList<MonthlyPostCountItem>> CountMonthlyPostsAsync(
		int numMonths, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>()
			.GroupBy(x => new { x.PostedDate.Year, x.PostedDate.Month })
			.Select(g => new MonthlyPostCountItem()
			{
				Year = g.Key.Year,
				Month = g.Key.Month,
				PostCount = g.Count(x => x.Published)
			})
			.OrderByDescending(x => x.Year)
			.ThenByDescending(x => x.Month)
			.ToListAsync(cancellationToken);
	}

	public async Task<Category> GetCategoryAsync(
		string slug, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Category>()
			.FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
	}

	public async Task<IList<CategoryItem>> GetCategoriesAsync(
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Category>()
			.OrderBy(x => x.Name)
			.Select(x => new CategoryItem()
			{
				Id = x.Id,
				Name = x.Name,
				UrlSlug = x.UrlSlug,
				Description = x.Description,
				PostCount = x.Posts.Count(p => p.Published)
			})
			.ToListAsync(cancellationToken);
	}

	public async Task<Tag> GetTagAsync(
		string slug, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Tag>()
			.FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
	}

	public async Task<IList<TagItem>> GetTagsAsync(
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Tag>()
			.OrderBy(x => x.Name)
			.Select(x => new TagItem()
			{
				Id = x.Id,
				Name = x.Name,
				UrlSlug = x.UrlSlug,
				Description = x.Description,
				PostCount = x.Posts.Count(p => p.Published)
			})
			.ToListAsync(cancellationToken);
	}

	public async Task<Post> GetPostAsync(
		int year, int month, string slug, 
		CancellationToken cancellationToken = default)
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

	public async Task<Post> GetPostByIdAsync(
		int postId, bool includeDetails = false, 
		CancellationToken cancellationToken = default)
	{
		if (!includeDetails)
		{
			return await _context.Set<Post>().FindAsync(postId);
		}

		return await _context.Set<Post>()
			.Include(x => x.Category)
			.Include(x => x.Tags)
			.FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
	}

	public async Task<bool> TogglePublishedFlagAsync(
		int postId, CancellationToken cancellationToken = default)
	{
		var post = await _context.Set<Post>().FindAsync(postId);

		if (post is null) return false;

		post.Published = !post.Published;
		await _context.SaveChangesAsync(cancellationToken);

		return post.Published;
	}

	public async Task<IList<Post>> GetPopularArticlesAsync(
		int numPosts, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>()
			.OrderBy(x => Guid.NewGuid())
			.Take(numPosts)
			.ToListAsync(cancellationToken);
	}

	public async Task<IPagedList<T>> GetPagedPostsAsync<T>(
		PostQuery condition, 
		IPagingParams pagingParams, 
		Func<IQueryable<Post>, IQueryable<T>> mapper)
	{
		var posts = FilterPosts(condition);
		var projectedPosts = mapper(posts);

		return await PagedList<T>.CreateAsync(projectedPosts, pagingParams);
	}

	public async Task<Post> CreateOrUpdatePostAsync(
		Post post, IEnumerable<string> tags, 
		CancellationToken cancellationToken = default)
	{
		if (post.Id > 0)
		{
			await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
		}
		else
		{
			post.Tags = new List<Tag>();
		}

		foreach (var tagName in tags)
		{
			if (string.IsNullOrWhiteSpace(tagName)) continue;
			if (post.Tags.Any(x => x.Name == tagName)) continue;

			var tag = await _context.Set<Tag>()
				.FirstOrDefaultAsync(x => x.Name == tagName, cancellationToken);

			if (tag == null)
			{
				var slug = Regex.Replace(
					tagName.ToLower().Trim(), "[^a-z0-9]", "-");

				tag = new Tag()
				{
					Name = tagName,
					Description = tagName,
					UrlSlug = slug.Trim(' ', '-')
				};

			}
		
			post.Tags.Add(tag);
		}

		post.Tags = post.Tags.Where(t => tags.Contains(t.Name)).ToList();

		if (post.Id > 0)
			_context.Update(post);
		else
			_context.Add(post);

		await _context.SaveChangesAsync(cancellationToken);

		return post;
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

		if (condition.NotPublished)
		{
			posts = posts.Where(x => !x.Published);
		}

		if (condition.CategoryId > 0)
		{
			posts = posts.Where(x => x.CategoryId == condition.CategoryId);
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