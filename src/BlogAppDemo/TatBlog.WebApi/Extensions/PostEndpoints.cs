using System.Net;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Extensions;

public static class PostEndpoints
{
	public static WebApplication MapPostEndpoints(this WebApplication app)
	{
		var routeGroupBuilder = app.MapGroup("/api/posts");

		routeGroupBuilder.MapGet("/", GetPosts)
			.WithName("GetPosts")
			//.Produces<PaginationResult<PostDto>>();
			.Produces<ApiResponse<PaginationResult<PostDto>>>();

		routeGroupBuilder.MapGet("/random/{limit:int}", GetRandomPosts)
			.WithName("GetRandomPosts")
			//.Produces<PaginationResult<PostDto>>();
			.Produces<ApiResponse<IEnumerable<PostDto>>>();

		routeGroupBuilder.MapGet("/featured/{limit:int}", GetFeaturedPosts)
			.WithName("GetFeaturedPosts")
			//.Produces<PaginationResult<PostDto>>();
			.Produces<ApiResponse<IEnumerable<PostDto>>>();

		routeGroupBuilder.MapGet("/archives/{year:int}/{month:int}", GetArchivedPosts)
			.WithName("GetArchivedPosts")
			//.Produces<PaginationResult<PostDto>>();
			.Produces<ApiResponse<PaginationResult<PostDto>>>();

		routeGroupBuilder.MapGet("/archives/{limit:int}", GetArchives)
			.WithName("GetArchives")
			//.Produces<PaginationResult<PostDto>>();
			.Produces<ApiResponse<IEnumerable<MonthlyPostCountItem>>>();

		routeGroupBuilder.MapGet("/{id:int}", GetPostDetails)
			.WithName("GetPostById")
			//.Produces<PostDetail>()
			//.Produces(404);
			.Produces<ApiResponse<PostDetail>>();

		routeGroupBuilder.MapGet("/detail/{slug:regex(^[a-z0-9_-]+$)}", GetPostBySlug)
			.WithName("GetPostBySlug")
			//.Produces<PostDetail>()
			//.Produces(404);
			.Produces<ApiResponse<PostDetail>>();

		return app;
	}

	private static async Task<IResult> GetPosts(
		//[FromQuery] string keyword,
		//[AsParameters] PagingModel model,
		[AsParameters] PostFilterModel model,
		IBlogRepository blogRepository,
		IMapper mapper)
	{
		var postQuery = mapper.Map<PostQuery>(model);
		var postsList = await blogRepository.GetPagedPostsAsync(
			postQuery, model, posts => posts.ProjectToType<PostDto>());

		var paginationResult = new PaginationResult<PostDto>(postsList);

		return Results.Ok(ApiResponse.Success(paginationResult));
	}

	private static async Task<IResult> GetRandomPosts(
		[FromRoute(Name = "limit")] int numPosts,
		IBlogRepository blogRepository,
		IMapper mapper)
	{
		if (numPosts < 1) numPosts = 5;

		var posts = await blogRepository.GetRandomArticlesAsync(
			numPosts, posts => posts.ProjectToType<PostDto>());

		return Results.Ok(ApiResponse.Success(posts));
	}

	private static async Task<IResult> GetFeaturedPosts(
		[FromRoute(Name = "limit")] int numPosts,
		IBlogRepository blogRepository,
		IMapper mapper)
	{
		if (numPosts < 1) numPosts = 5;

		var posts = await blogRepository.GetPopularArticlesAsync(
			numPosts, posts => posts.ProjectToType<PostDto>());

		return Results.Ok(ApiResponse.Success(posts));
	}

	private static async Task<IResult> GetArchivedPosts(
		[FromRoute] int year,
		[FromRoute] int month,
		[AsParameters] PagingModel model,
		IBlogRepository blogRepository)
	{
		var postQuery = new PostQuery()
		{
			Year = year,
			Month = month,
			PublishedOnly = true
		};

		var postsList = await blogRepository.GetPagedPostsAsync(
			postQuery, model, posts => posts.ProjectToType<PostDto>());

		var paginationResult = new PaginationResult<PostDto>(postsList);

		return Results.Ok(ApiResponse.Success(paginationResult));
	}

	private static async Task<IResult> GetArchives(
		[FromRoute(Name = "limit")] int numMonths,
		IBlogRepository blogRepository)
	{
		if (numMonths < 1) numMonths = 12;

		var archives = await blogRepository.CountMonthlyPostsAsync(numMonths);

		return Results.Ok(ApiResponse.Success(archives));
	}

	private static async Task<IResult> GetPostDetails(
		int id,
		IBlogRepository blogRepository,
		IMapper mapper)
	{
		var post = await blogRepository.GetPostByIdAsync(id, true);
		return post == null
			? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không tìm thấy bài viết"))
			: Results.Ok(ApiResponse.Success(mapper.Map<PostDetail>(post)));
	}

	private static async Task<IResult> GetPostBySlug(
		[FromRoute] string slug,
		IBlogRepository blogRepository,
		IMapper mapper)
	{
		var post = await blogRepository.GetPostAsync(slug);

		return post == null
			? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không tìm thấy bài viết"))
			: Results.Ok(ApiResponse.Success(mapper.Map<PostDetail>(post)));
	}
}