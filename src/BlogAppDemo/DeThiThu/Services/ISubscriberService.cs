using DeThiThu.Entities;

namespace DeThiThu.Services;

public interface ISubscriberService
{
	Task<bool> AddSubscriberAsync(string email,
		CancellationToken cancellationToken = default);

	Task<List<Subscriber>> GetSubscribersAsync(
		DateTime? fromDate = null, int? n = 10,
		CancellationToken cancellationToken = default);
}