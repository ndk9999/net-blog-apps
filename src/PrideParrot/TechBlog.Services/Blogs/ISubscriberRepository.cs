using TechBlog.Core.Contracts;
using TechBlog.Core.Entities;

namespace TechBlog.Services.Blogs;

public interface ISubscriberRepository
{
	Task<Subscriber> SubscribeAsync(
		string email, 
		CancellationToken cancellationToken = default);

	Task<bool> UnsubscribeAsync(
		string email,
		string reason,
		bool voluntary,
		CancellationToken cancellationToken = default);

	Task<Subscriber> GetSubscriberByEmailAsync(
		string email,
		CancellationToken cancellationToken = default);

	Task<Subscriber> GetSubscriberByIdAsync(
		int subscriberId,
		CancellationToken cancellationToken = default);

	Task<bool> BlockSubscriberAsync(
		int subscriberId,
		string reason,
		string notes,
		bool involuntary = false,
		CancellationToken cancellationToken = default);

	Task<bool> DeleteSubscriberAsync(
		int subscriberId,
		CancellationToken cancellationToken = default);

	Task<IPagedList<Subscriber>> GetNewsletterReceiversAsync(
		int pageNumber,
		int pageSize,
		CancellationToken cancellationToken = default);

	Task<IPagedList<Subscriber>> SearchSubscribersAsync(
		IPagingParams pagingParams,
		string keyword = null,
		bool unsubscribed = false,
		bool involuntary = false,
		CancellationToken cancellationToken = default);
}