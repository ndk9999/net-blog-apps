using Microsoft.AspNetCore.WebUtilities;

namespace TechBlog.Services.Media;

public class AmazonStorageMediaManager : IMediaManager
{
	public Task<string> SaveFileAsync(
		Stream buffer, 
		string originalFileName, 
		string contentType,
		CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<string> SaveFileAsync(
		MultipartReader reader,
		CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteFileAsync(string path, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
}