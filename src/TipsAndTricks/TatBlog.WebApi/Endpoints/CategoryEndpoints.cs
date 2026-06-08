//using FluentValidation;
//using Mapster;
//using MapsterMapper;
//using Microsoft.AspNetCore.Mvc;
//using TatBlog.Core.Collections;
//using TatBlog.Core.DTO;
//using TatBlog.Core.Entities;
//using TatBlog.Services.Blogs;
//using TatBlog.WebApi.Filters;
//using TatBlog.WebApi.Models;

//namespace TatBlog.WebApi.Endpoints;

//public static class CategoryEndpoints
//{
//    public static WebApplication MapCategoryEndpoints(this WebApplication app)
//    {
//        var routeGroupBuilder = app.MapGroup("/api/categories");

//        routeGroupBuilder.MapGet("/", GetCategories)
//            .WithName("GetCategories")
//            .Produces<PaginationResult<CategoryItem>>();

//        routeGroupBuilder.MapGet("/{id:int}", GetCategoryDetails)
//            .WithName("GetCategoryById")
//            .Produces<CategoryItem>()
//            .Produces(404);

//        //routeGroupBuilder.MapGet("/{id:int}/posts", GetPostsByCategory)
//        //	.WithName("GetPostsByCategoryId")
//        //	.Produces<PaginationResult<PostDto>>();

//        routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByCategorySlug)
//            .WithName("GetPostsByCategorySlug")
//            .Produces<PaginationResult<PostDto>>();

//        routeGroupBuilder.MapPost("/", AddCategory)
//            .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
//            .WithName("AddNewCategory")
//            .Produces(201)
//            .Produces(400)
//            .Produces(409);

//        routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
//            .WithName("UpdateCategory")
//            .Produces(204)
//            .Produces(400)
//            .Produces(409);

//        routeGroupBuilder.MapDelete("/{id:int}", DeleteCategory)
//            .WithName("DeleteCategory")
//            .Produces(204)
//            .Produces(404);

//        return app;
//    }

//    private static async Task<IResult> GetCategories(
//        IBlogRepository blogRepository)
//    {
//        var categories = await blogRepository.GetCategoriesAsync();
//        return Results.Ok(categories);
//    }

//    private static async Task<IResult> GetCategoryDetails(
//        int id,
//        IBlogRepository blogRepository,
//        IMapper mapper)
//    {
//        var category = await blogRepository.GetCategoryByIdAsync(id);
//        return category == null
//            ? Results.NotFound($"Could not find category {id}")
//            : Results.Ok(mapper.Map<CategoryItem>(category));
//    }

//    private static async Task<IResult> GetPostsByCategory(
//        int id,
//        [AsParameters] PagingModel pagingModel,
//        IBlogRepository blogRepository)
//    {
//        var postQuery = new PostQuery()
//        {
//            CategoryId = id,
//            PublishedOnly = true
//        };

//        var postsList = await blogRepository.GetPagedPostsAsync(
//            postQuery, pagingModel,
//            posts => posts.ProjectToType<PostDto>());

//        var paginationResult = new PaginationResult<PostDto>(postsList);

//        return Results.Ok(paginationResult);
//    }

//    private static async Task<IResult> GetPostsByCategorySlug(
//        [FromRoute] string slug,
//        [AsParameters] PagingModel pagingModel,
//        IBlogRepository blogRepository)
//    {
//        var postQuery = new PostQuery()
//        {
//            CategorySlug = slug,
//            PublishedOnly = true
//        };

//        var postsList = await blogRepository.GetPagedPostsAsync(
//            postQuery, pagingModel,
//            posts => posts.ProjectToType<PostDto>());

//        var paginationResult = new PaginationResult<PostDto>(postsList);

//        return Results.Ok(paginationResult);
//    }

//    private static async Task<IResult> AddCategory(
//        CategoryEditModel model,
//        //IValidator<CategoryEditModel> validator, 
//        IBlogRepository blogRepository,
//        IMapper mapper)
//    {
//        //var validationResult = await validator.ValidateAsync(model);

//        //if (!validationResult.IsValid)
//        //{
//        //	return Results.BadRequest(validationResult.Errors.ToResponse());
//        //}

//        if (await blogRepository.IsCategorySlugExistedAsync(0, model.UrlSlug))
//        {
//            return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
//        }

//        var category = mapper.Map<Category>(model);
//        await blogRepository.CreateOrUpdateCategoryAsync(category);

//        return Results.CreatedAtRoute("GetCategoryById", new { category.Id }, category);
//    }

//    private static async Task<IResult> UpdateCategory(
//        int id,
//        CategoryEditModel model,
//        IValidator<CategoryEditModel> validator,
//        IBlogRepository blogRepository,
//        IMapper mapper)
//    {
//        var validationResult = await validator.ValidateAsync(model);

//        if (!validationResult.IsValid)
//        {
//            return Results.BadRequest(validationResult.Errors.ToResponse());
//        }

//        if (await blogRepository.IsCategorySlugExistedAsync(id, model.UrlSlug))
//        {
//            return Results.Conflict($"Slug '{model.UrlSlug}' đã được sử dụng");
//        }

//        var category = mapper.Map<Category>(model);
//        category.Id = id;

//        return await blogRepository.CreateOrUpdateCategoryAsync(category)
//            ? Results.NoContent()
//            : Results.NotFound();
//    }

//    private static async Task<IResult> DeleteCategory(
//        int id,
//        IBlogRepository blogRepository)
//    {
//        return await blogRepository.DeleteCategoryAsync(id)
//            ? Results.NoContent()
//            : Results.NotFound($"Không tìm thấy chủ đề có id = {id}");
//    }
//}