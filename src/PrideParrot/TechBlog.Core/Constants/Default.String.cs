namespace TechBlog.Core.Constants;

public sealed partial class Default
{
	public sealed class PostQueryPurpose
	{
		public const string LatestPosts = "latest";
		public const string FilterByCategory = "category";
		public const string FilterByTag = "tag";
		public const string SearchByKeyword = "search";
	}

	public sealed class MailingServiceNames
	{
		public const string Smtp = "smtp";
		public const string SendGrid = "sendgrid";
	}
}