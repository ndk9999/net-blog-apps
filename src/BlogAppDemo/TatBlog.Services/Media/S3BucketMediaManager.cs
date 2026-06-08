namespace TatBlog.Services.Media;

public class S3BucketMediaManager : IMediaManager
{
	public Task<string> SaveFileAsync(Stream buffer, string originalFileName, string contentType,
		CancellationToken cancellationToken = default)
	{
		var ext = Path.GetExtension(originalFileName);
		return Task.FromResult($"{Guid.NewGuid():N}{ext}");
	}
}