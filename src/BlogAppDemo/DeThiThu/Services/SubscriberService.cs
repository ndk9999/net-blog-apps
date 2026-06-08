using DeThiThu.Contexts;
using DeThiThu.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeThiThu.Services;

public class SubscriberService : ISubscriberService
{
	private readonly CandyContext _context;

	public SubscriberService(CandyContext context)
	{
		_context = context;
	}

	public async Task<bool> AddSubscriberAsync(string email,
		CancellationToken cancellationToken = default)
	{
		if (await _context.Subscribers
			    .AnyAsync(x => x.Email == email, cancellationToken))
			return false;

		_context.Subscribers.Add(new Subscriber()
		{
			Email = email,
			SubscribedDate = DateTime.Now
		});

		await _context.SaveChangesAsync(cancellationToken);

		return true;
	}

public async Task<List<Subscriber>> GetSubscribersAsync(
	DateTime? fromDate = null, int? n = 10,
	CancellationToken cancellationToken = default)
{
	var rows = n > 0 ? n.Value : 10;

	return (fromDate == null || fromDate.Value == DateTime.MinValue)
		? await _context.Subscribers
			.OrderByDescending(x => x.SubscribedDate)
			.Take(rows)
			.ToListAsync(cancellationToken)
		: await _context.Subscribers
			.Where(x => x.SubscribedDate >= fromDate)
			.OrderByDescending(x => x.SubscribedDate)
			.ToListAsync(cancellationToken);
}
}