using Microsoft.EntityFrameworkCore;
using TechBlog.Core.Contracts;
using TechBlog.Core.Entities;
using TechBlog.Data.Contexts;
using TechBlog.Services.Extensions;

namespace TechBlog.Services.Blogs;

public class SubscriberRepository : ISubscriberRepository
{
	private readonly BlogDbContext _context;

	public SubscriberRepository(BlogDbContext context)
	{
		_context = context;
	}

	public async Task<Subscriber> SubscribeAsync(
		string email, 
		CancellationToken cancellationToken = default)
	{
		var subscriber = await GetSubscriberByEmailAsync(email, cancellationToken);

		// In case the email was already in our system
		if (subscriber is not null)
		{
			// Do nothing if it is not unsubscribed or involuntary
			if (subscriber.UnsubscribedDate == null || !subscriber.Voluntary)
				return subscriber;

			// If user unsubscribed voluntary, release lock status
			subscriber.UnsubscribedDate = null;
			subscriber.UnsubscribedReason = null;
			subscriber.Voluntary = false;
			subscriber.Notes = $"Subscribe again for newsletter on {DateTime.Now}";

			await _context.SaveChangesAsync(cancellationToken);
			return subscriber;
		}

		subscriber = new Subscriber()
		{
			Email = email,
			SubscribedDate = DateTime.Now
		};

		_context.Set<Subscriber>().Add(subscriber);
		await _context.SaveChangesAsync(cancellationToken);

		return subscriber;
	}

	public async Task<bool> UnsubscribeAsync(
		string email, 
		string reason, 
		bool voluntary, 
		CancellationToken cancellationToken = default)
	{
		var subscriber = await GetSubscriberByEmailAsync(email, cancellationToken);

		if (subscriber is null)
			return false;

		if (subscriber.UnsubscribedDate != null)
			return true;

		subscriber.UnsubscribedDate = DateTime.Now;
		subscriber.UnsubscribedReason = reason;
		subscriber.Voluntary = voluntary;

		await _context.SaveChangesAsync(cancellationToken);

		return true;
	}

	public async Task<Subscriber> GetSubscriberByEmailAsync(
		string email, 
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Subscriber>()
			.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
	}

	public async Task<IPagedList<Subscriber>> GetNewsletterReceiversAsync(
		int pageNumber,
		int pageSize,
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Subscriber>()
			.Where(x => x.UnsubscribedDate == null)
			.ToPagedListAsync(
				pageNumber, 
				pageSize, 
				sortOrder: "ASC",
				cancellationToken: cancellationToken);
	}
}