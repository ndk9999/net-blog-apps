using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace TechBlog.Services.Media;

public class LocalFileSystemMediaManager : IMediaManager
{
	private const string PicturesFolder = "uploads/pictures/{0}{1}";
	private const string VideosFolder = "uploads/videos/{0}{1}";
	private const string DocumentsFolder = "uploads/documents/{0}{1}";
	private const string OthersFolder = "uploads/others/{0}{1}";

	private static readonly string[] ImageMimeTypes = new[]
	{
		"image/jpg", "image/jpeg", "image/pjpeg", "image/gif", "image/x-png", "image/png"
	};
	private static readonly string[] ImageFileExts = new[]
	{
		".jpg", ".jpeg", ".gif", ".png"
	};

	private static readonly string[] VideoMimeTypes = new[]
	{
		"video/mp4", "video/mpeg", "video/x-ms-wmv", "audio/mpeg", "audio/wav", "audio/x-ms-wma"
	};
	private static readonly string[] VideoFileExts = new[]
	{
		".mp4", ".mpeg", ".wmv", ".mp3", ".wav", ".wma"
	};

	private static readonly string[] DocMimeTypes = new[]
	{
		"text/plain", 
		"application/msword",
		"application/vnd.openxmlformats-officedocument.wordprocessingml.document",
		"application/pdf",
		"application/vnd.ms-excel",
		"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
	};
	private static readonly string[] DocFileExts = new[]
	{
		"txt", "doc", "docx", "pdf", "xls", "xlsx"
	};


	private readonly ILogger<LocalFileSystemMediaManager> _logger;


	public LocalFileSystemMediaManager(ILogger<LocalFileSystemMediaManager> logger)
	{
		_logger = logger;
	}


	public async Task<string> SaveFileAsync(
		Stream buffer,
		string originalFileName,
		string contentType,
		CancellationToken cancellationToken = default)
	{
		try
		{
			if (!buffer.CanRead || !buffer.CanSeek || buffer.Length == 0) return null;
			
			var fileExt = Path.GetExtension(originalFileName).ToLower();
			var returnedFilePath = CreateFilePath(fileExt, contentType.ToLower());
			var fullPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot", returnedFilePath));

			// Make sure we at at beginning of source stream
			buffer.Position = 0;

			await using var fileStream = new FileStream(fullPath, FileMode.Create);
			await buffer.CopyToAsync(fileStream, cancellationToken);

			return returnedFilePath;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Could not save file '{originalFileName}'.");
			return null;
		}
	}

	public async Task<string> SaveFileAsync(
		MultipartReader reader,
		CancellationToken cancellationToken = default)
	{
		try
		{
			string returnedFilePath = null;
			var section = await reader.ReadNextSectionAsync(cancellationToken);

			while (section != null)
			{
				if (ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition) &&
				    contentDisposition.DispositionType.Equals("form-data") &&
				    TryGetFileName(contentDisposition, out var originalFileName))
				{
					var fileExt = Path.GetExtension(originalFileName).ToLower();
					returnedFilePath = CreateFilePath(fileExt);

					var fullPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot", returnedFilePath));

					await using var memoryStream = new MemoryStream();
					await section.Body.CopyToAsync(memoryStream, cancellationToken);

					var buffer = memoryStream.ToArray();

					await using var fileStream = File.Create(fullPath);
					await fileStream.WriteAsync(buffer, cancellationToken);
				}

				section = await reader.ReadNextSectionAsync(cancellationToken);
			}

			return returnedFilePath;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Could not save large file from stream.");
			return null;
		}
	}

	public Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(filePath)) Task.FromResult(true);

			var fullPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot", filePath));
			File.Delete(fullPath);

			return Task.FromResult(true);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Could not delete file '{filePath}'.");
			return Task.FromResult(false);
		}
	}

	private string CreateFilePath(string fileExt, string contentType = null)
	{
		if (IsImageFile(fileExt, contentType))
			return string.Format(PicturesFolder, Guid.NewGuid().ToString("N"), fileExt);

		if (IsVideoFile(fileExt, contentType))
			return string.Format(VideosFolder, Guid.NewGuid().ToString("N"), fileExt);

		if (IsDocumentFile(fileExt, contentType))
			return string.Format(DocumentsFolder, Guid.NewGuid().ToString("N"), fileExt);

		return string.Format(OthersFolder, Guid.NewGuid().ToString("N"), fileExt);
	}

	private bool IsImageFile(string fileExt, string contentType = null)
	{
		return (contentType == null || ImageMimeTypes.Contains(contentType)) && ImageFileExts.Contains(fileExt);
	}

	private bool IsVideoFile(string fileExt, string contentType = null)
	{
		return (contentType == null || VideoMimeTypes.Contains(contentType)) && VideoFileExts.Contains(fileExt);
	}

	private bool IsDocumentFile(string fileExt, string contentType = null)
	{
		return (contentType == null || DocMimeTypes.Contains(contentType)) && DocFileExts.Contains(fileExt);
	}

	private bool TryGetFileName(ContentDispositionHeaderValue contentDisposition, out string fileName)
	{
		fileName = contentDisposition.FileName.Value;

		if (!string.IsNullOrWhiteSpace(fileName))
		{
			fileName = contentDisposition.FileNameStar.Value;
		}

		return !string.IsNullOrWhiteSpace(fileName);
	}
}