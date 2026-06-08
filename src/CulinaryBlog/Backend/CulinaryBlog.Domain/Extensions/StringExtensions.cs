namespace CulinaryBlog.Domain.Extensions;

public static class StringExtensions
{
	// Đơn giản hóa: lowercase + replace space → dash + loại ký tự không an toàn.
	// Production: Có thể dùng Slugify NuGet package hoặc xử lý Unicode đúng cách.
	public static string Slugify(this string input)
	{
		if (string.IsNullOrWhiteSpace(input))
			return string.Empty;

		return input.ToLowerInvariant()
			.Replace(" ", "-")
			.Replace("_", "-")
			.Replace("--", "-")
			.Replace("đ", "d");
	}
}