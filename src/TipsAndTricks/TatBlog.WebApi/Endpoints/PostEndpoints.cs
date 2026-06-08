using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints;

public static class PostEndpoints
{
    public static WebApplication MapPostEndpoints(this WebApplication app)
    {
        var routeGroupBuilder = app.MapGroup("/api/posts");

        routeGroupBuilder.MapGet("/", GetPosts)
            .WithName("GetPosts")
            .Produces<PaginationResult<PostDto>>();

        routeGroupBuilder.MapGet("/{id:int}", GetPostDetails)
            .WithName("GetPostById")
            .Produces<PostDetail>()
            .Produces(404);

        routeGroupBuilder.MapGet("/detail/{slug:regex(^[a-z0-9_-]+$)}", GetPostBySlug)
            .WithName("GetPostBySlug")
            .Produces<PostDetail>()
            .Produces(404);

        return app;
    }

    private static async Task<IResult> GetPosts(
        [FromQuery] string keyword,
        [AsParameters] PagingModel model,
        IBlogRepository blogRepository)
    {
        var postQuery = new PostQuery()
        {
            Keyword = keyword,
            PublishedOnly = true
        };

        var postsList = await blogRepository.GetPagedPostsAsync(
            postQuery, model, posts => posts.ProjectToType<PostDto>());

        var paginationResult = new PaginationResult<PostDto>(postsList);

        return Results.Ok(paginationResult);
    }

    private static async Task<IResult> GetPostDetails(
        int id,
        IBlogRepository blogRepository,
        IMapper mapper)
    {
        var post = await blogRepository.GetPostByIdAsync(id, true);
        return post == null
            ? Results.NotFound($"Không tìm thấy bài viết có mã số {id}")
            : Results.Ok(mapper.Map<PostDetail>(post));
    }

    private static async Task<IResult> GetPostBySlug(
        [FromRoute] string slug,
        IBlogRepository blogRepository,
        IMapper mapper)
    {
        var post = await blogRepository.GetPostAsync(slug);
        return post == null
            ? Results.NotFound("Không tìm thấy bài viết")
            : Results.Ok(mapper.Map<PostDetail>(post));
    }
}