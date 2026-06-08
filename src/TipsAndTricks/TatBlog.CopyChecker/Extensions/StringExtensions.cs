namespace TatBlog.CopyChecker.Extensions;

public static class StringExtensions
{
	public static bool IsEmpty(this string input)
	{
		return string.IsNullOrWhiteSpace(input);
	}

	public static string Quote(this string input)
	{
		return $"\"{input}\"";
	}

	public static string RemoveCommas(this string input)
	{
		return input.Replace(',', ' ');
	}

	public static string Join(this IEnumerable<string> values, string separator = ",")
	{
		return string.Join(separator, values);
	}

	public static string QuoteAndJoin(this IEnumerable<string> values, string separator = ",")
	{
		return values.Select(x => x.Quote()).Join(separator);
	}
}