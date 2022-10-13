using Microsoft.EntityFrameworkCore;
using TechBlog.Core.Constants;
using TechBlog.Core.DTO;
using TechBlog.Core.Entities;
using TechBlog.Data.Contexts;

namespace TechBlog.Services.Blogs;

public class NewsletterRepository : INewsletterRepository
{
	private readonly BlogDbContext _context;

	public NewsletterRepository(BlogDbContext context)
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

	public async Task<IList<int>> GetNewsletterReceiversAsync(
		int numberOfSubscribers,
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Newsletter>()
			.Where(x => x.Status == NewsletterStatus.Pending)
			.Select(x => x.SubscriberId)
			.Distinct()
			.Take(numberOfSubscribers)
			.ToListAsync(cancellationToken);
	}

	public async Task<IList<PostItem>> GetNewPostsForSubscriberAsync(
		int subscriberId,
		CancellationToken cancellationToken = default)
	{
		return await _context.Set<Newsletter>()
			.Where(x => x.SubscriberId == subscriberId && 
			            x.Status == NewsletterStatus.Pending)
			.Select(x => new PostItem()
			{
				Id = x.Post.Id,
				Title = x.Post.Title,
				ShortDescription = x.Post.ShortDescription,
				UrlSlug = x.Post.UrlSlug,
				ImageUrl = x.Post.ImageUrl,
				PostedDate = x.Post.PostedDate
			})
			.OrderBy(x => x.PostedDate)
			.ToListAsync(cancellationToken);
	}

	public async Task MarkNewslettersAsSentAsync(
		int subscriberId,
		IEnumerable<int> postIdList,
		CancellationToken cancellationToken = default)
	{
		var notes = $"Sent on {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

		await _context.Set<Newsletter>()
			.Where(x => postIdList.Contains(x.PostId) && x.SubscriberId == subscriberId)
			.UpdateFromQueryAsync(x => new Newsletter()
			{
				Status = NewsletterStatus.Sent,
				Notes = notes
			}, cancellationToken);
	}

	public async Task DeleteSentNewslettersAsync(
		int? subscriberId,
		int? postId,
		CancellationToken cancellationToken = default)
	{
		var deleteQuery = _context.Set<Newsletter>()
			.Where(x => x.Status == NewsletterStatus.Sent);

		if (subscriberId > 0)
			deleteQuery = deleteQuery.Where(x => x.SubscriberId == subscriberId);

		if (postId > 0)
			deleteQuery = deleteQuery.Where(x => x.PostId == postId);

		await deleteQuery.DeleteFromQueryAsync(cancellationToken);
	}
}