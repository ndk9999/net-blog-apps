using Microsoft.EntityFrameworkCore;
using TechBlog.Core.Contexts;
using TechBlog.Core.Entities;

namespace TechBlog.Core.Repositories;

public class BlogRepository : IBlogRepository
{
	private readonly BlogDbContext _context;

	public BlogRepository(BlogDbContext context)
	{
		_context = context;
	}

	public async Task<IList<Post>> GetPosts(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>()
			.Include(x => x.Category)
			.Include(x => x.Tags)
			.Where(x => x.Published)
			.OrderByDescending(x => x.PostedDate)
			.Skip(pageNumber * pageSize)
			.Take(pageSize)
			.ToListAsync(cancellationToken: cancellationToken);
	}

	public async Task<int> CountTotalPosts(CancellationToken cancellationToken = default)
	{
		return await _context.Set<Post>()
			.Where(x => x.Published)
			.CountAsync(cancellationToken: cancellationToken);
	}
}