namespace TechBlog.Web.Models
{
	public class ErrorViewModel
	{
		public string RequestId { get; init; }

		public string Message { get; init; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}
}