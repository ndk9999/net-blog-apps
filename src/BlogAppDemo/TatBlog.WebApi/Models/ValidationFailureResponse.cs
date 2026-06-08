namespace TatBlog.WebApi.Models;

public class ValidationFailureResponse
{
	public IEnumerable<string> Errors { get; set; } = new List<string>();
}