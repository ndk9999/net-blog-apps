using Newtonsoft.Json;
using TechBlog.Web.Models;

namespace TechBlog.Web.Extensions;

public static class AlertMessageExtensions
{
	public static string AsJson(this AlertMessage alertMessage)
	{
		return JsonConvert.SerializeObject(alertMessage);
	}
}