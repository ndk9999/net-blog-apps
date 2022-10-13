using TechBlog.Core.DTO;
using TechBlog.Core.Entities;

namespace TechBlog.Services.Blogs;

public interface INewsletterRepository
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

	Task<IList<int>> GetNewsletterReceiversAsync(
		int numberOfSubscribers,
		CancellationToken cancellationToken = default);

	Task<IList<PostItem>> GetNewPostsForSubscriberAsync(
		int subscriberId,
		CancellationToken cancellationToken = default);

	Task MarkNewslettersAsSentAsync(
		int subscriberId,
		IEnumerable<int> postIdList,
		CancellationToken cancellationToken = default);

	Task DeleteSentNewslettersAsync(
		int? subscriberId,
		int? postId,
		CancellationToken cancellationToken = default);
}