using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TechBlog.Web.Extensions;

public static class ModelStateExtensions
{
	public static IList<string> GetErrorMessages(this ModelStateDictionary modelState)
	{
		return modelState.Values
			.SelectMany(x => x.Errors.Select(e => e.ErrorMessage))
			.ToList();
	}

	public static string GetJoinedErrorMessages(this ModelStateDictionary modelState, string separator = ", ")
	{
		return string.Join(separator, modelState.GetErrorMessages());
	}
}