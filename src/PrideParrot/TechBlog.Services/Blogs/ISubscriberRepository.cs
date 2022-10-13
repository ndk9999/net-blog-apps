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

	Task<IPagedList<Subscriber>> GetNewsletterReceiversAsync(
		int pageNumber,
		int pageSize,
		CancellationToken cancellationToken = default);
}