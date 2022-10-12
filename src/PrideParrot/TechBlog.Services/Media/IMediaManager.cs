using Microsoft.AspNetCore.WebUtilities;

namespace TechBlog.Services.Media;

public interface IMediaManager
{
	Task<string> SaveFileAsync(
		Stream buffer, 
		string originalFileName,
		string contentType,
		CancellationToken cancellationToken = default);

	Task<string> SaveFileAsync(
		MultipartReader reader,
		CancellationToken cancellationToken = default);

	Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);
}