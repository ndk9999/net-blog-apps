namespace TechBlog.Services.Rss;

public interface IFeedProvider
{
	Task<byte[]> CreateAsync(CancellationToken cancellationToken = default);
}