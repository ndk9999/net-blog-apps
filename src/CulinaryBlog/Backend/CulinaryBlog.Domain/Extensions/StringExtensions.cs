using Slugify;

namespace CulinaryBlog.Domain.Extensions;

public static class StringExtensions
{
	private static readonly SlugHelper SlugHelper = new();

	// Đơn giản hóa: lowercase + replace space → dash + loại ký tự không an toàn.
	// Production: Có thể dùng Slugify NuGet package hoặc xử lý Unicode đúng cách.
	public static string ToSlug(this string input)
	{
		return string.IsNullOrWhiteSpace(input) ? string.Empty : SlugHelper.GenerateSlug(input);
	}
}