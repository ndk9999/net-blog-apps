using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Extensions;

public static class TagEndpoints
{
	public static WebApplication MapTagEndpoints(this WebApplication app)
	{
		var routeGroupBuilder = app.MapGroup("/api/tags");

		routeGroupBuilder.MapGet("/", GetTags)
			.WithName("GetTags")
			.Produces<ApiResponse<IEnumerable<TagItem>>>();

		return app;
	}

	private static async Task<IResult> GetTags(
		IBlogRepository blogRepository)
	{
		var tagsList = await blogRepository.GetTagsAsync();
		return Results.Ok(ApiResponse.Success(tagsList));
	}
}