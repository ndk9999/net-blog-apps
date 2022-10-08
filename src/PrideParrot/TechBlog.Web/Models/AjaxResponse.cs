namespace TechBlog.Web.Models;

public class AjaxResponse
{
	public bool Success => !Messages.Any();

	public List<string> Messages { get; set; }

	public object Data { get; set; }

	private AjaxResponse()
	{
		Messages = new List<string>();
	}

	private AjaxResponse(IEnumerable<string> messages)
	{
		Messages = new List<string>();
	}

	public static AjaxResponse Ok(object data = null)
	{
		return new AjaxResponse()
		{
			Data = data
		};
	}

	public static AjaxResponse Error(params string[] messages)
	{
		return new AjaxResponse(messages);
	}

	public static AjaxResponse Error(IEnumerable<string> messages)
	{
		return new AjaxResponse(messages);
	}

}