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

	public async Task<Subscriber> GetSubscriberByIdAsync(
		int subscriberId,
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Subscriber>()
			.FirstOrDefaultAsync(x => x.Id == subscriberId, cancellationToken);
	}

	public async Task<bool> BlockSubscriberAsync(
		int subscriberId,
		string reason,
		string notes,
		bool involuntary = false,
		CancellationToken cancellationToken = default)
	{
		var subscriber = await GetSubscriberByIdAsync(subscriberId, cancellationToken);

		if (subscriber == null) return false;

		subscriber.UnsubscribedDate ??= DateTime.Now;
		subscriber.UnsubscribedReason = reason;
		subscriber.Notes = notes;
		subscriber.Voluntary = !involuntary;

		await _context.SaveChangesAsync(cancellationToken);

		return true;
	}

	public async Task<bool> DeleteSubscriberAsync(
		int subscriberId,
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Subscriber>()
			.Where(x => x.Id == subscriberId)
			.DeleteFromQueryAsync(cancellationToken) > 0;
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

	public async Task<IPagedList<Subscriber>> SearchSubscribersAsync(
		IPagingParams pagingParams,
		string keyword = null,
		bool unsubscribed = false,
		bool involuntary = false,
		CancellationToken cancellationToken = default)
	{
		IQueryable<Subscriber> subscriberQuery = _context.Set<Subscriber>();

		if (!string.IsNullOrWhiteSpace(keyword))
		{
			subscriberQuery = subscriberQuery.Where(x =>
				x.Email.Contains(keyword) || x.Notes.Contains(keyword) || x.UnsubscribedReason.Contains(keyword));
		}

		if (involuntary)
		{
			subscriberQuery = subscriberQuery.Where(x =>
				!x.Voluntary && x.UnsubscribedDate != null);
		}
		else if (unsubscribed)
		{
			subscriberQuery = subscriberQuery.Where(x => x.UnsubscribedDate != null);
		}

		return await subscriberQuery.ToPagedListAsync(pagingParams, cancellationToken);
	}
}