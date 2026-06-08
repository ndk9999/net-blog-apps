using MapsterMapper;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Extensions;

public static class SubscriberEndpoints
{
	public static WebApplication MapSubscriberEndpoints(this WebApplication app)
	{
		var routeGroupBuilder = app.MapGroup("/api/subscribers");

		routeGroupBuilder.MapPost("/", AddSubscriber)
			//.AddEndpointFilter<ValidatorFilter<NewSubscriberModel>>()
			.WithName("AddSubscriber")
			.Produces<ApiResponse<NewSubscriberModel>>();

		return app;
	}

	private static async Task<IResult> AddSubscriber(
		NewSubscriberModel model)
	{
		await Task.Delay(100);
		return Results.Ok(ApiResponse.Success(model));
	}
}