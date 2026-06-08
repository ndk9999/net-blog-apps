namespace TatBlog.Services.Media;

public interface IMediaManager
{
	Task<string> SaveFileAsync(
		Stream buffer,
		string originalFileName,
		string contentType,
		CancellationToken cancellationToken = default);
}