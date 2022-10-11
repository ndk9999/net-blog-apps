﻿using Microsoft.EntityFrameworkCore;
using SlugGenerator;
using TechBlog.Core.Collections;
using TechBlog.Core.Contracts;
using TechBlog.Core.DTO;
using TechBlog.Core.Entities;
using TechBlog.Data.Contexts;
using TechBlog.Services.Extensions;

namespace TechBlog.Services.Blogs;

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

	public async Task<Category> GetCategoryByIdAsync(int categoryId)
	{
		return await _context.Set<Category>().FindAsync(categoryId);
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

	public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
		IPagingParams pagingParams,
		CancellationToken cancellationToken = default)
	{
		var tagQuery = _context.Set<Category>()
			.Select(x => new CategoryItem()
			{
				Id = x.Id,
				Name = x.Name,
				UrlSlug = x.UrlSlug,
				Description = x.Description,
				PostCount = x.Posts.Count(p => p.Published)
			});

		return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
	}

	public async Task<Category> CreateOrUpdateCategoryAsync(
		Category category, CancellationToken cancellationToken = default)
	{
		if (category.Id > 0)
		{
			_context.Set<Category>().Update(category);
		}
		else
		{
			_context.Set<Category>().Add(category);
		}

		await _context.SaveChangesAsync(cancellationToken);

		return category;
	}

	public async Task<bool> IsCategorySlugExistedAsync(
		string categorySlug, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Category>().AnyAsync(x => x.UrlSlug == categorySlug);
	}

	public async Task<bool> DeleteCategoryAsync(
		int categoryId, CancellationToken cancellationToken = default)
	{
		var category = await _context.Set<Category>().FindAsync(categoryId);

		if (category is null) return false;

		_context.Set<Category>().Remove(category);
		var rowsCount = await _context.SaveChangesAsync(cancellationToken);

		return rowsCount > 0;
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

	public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
		IPagingParams pagingParams, CancellationToken cancellationToken = default)
	{
		var tagQuery = _context.Set<Tag>()
			.Select(x => new TagItem()
			{
				Id = x.Id,
				Name = x.Name,
				UrlSlug = x.UrlSlug,
				Description = x.Description,
				PostCount = x.Posts.Count(p => p.Published)
			});

		return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
	}

	public async Task<bool> DeleteTagAsync(
		int tagId, CancellationToken cancellationToken = default)
	{
		var tag = await _context.Set<Tag>().FindAsync(tagId);

		if (tag == null) return false;

		_context.Set<Tag>().Remove(tag);
		await _context.SaveChangesAsync(cancellationToken);

		return true;
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

		return await projectedPosts.ToPagedListAsync(pagingParams);
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
				tag = new Tag()
				{
					Name = tagName,
					Description = tagName,
					UrlSlug = tagName.GenerateSlug()
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

	public async Task<bool> IsPostSlugExistedAsync(
		string slug, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>()
			.AnyAsync(x => x.UrlSlug == slug, cancellationToken);
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