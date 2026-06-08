using CulinaryBlog.Application.Common.Models;
using CulinaryBlog.Application.DTOs;
using CulinaryBlog.Application.Features.Categories.Commands.CreateCategory;
using CulinaryBlog.Application.Features.Categories.Commands.DeleteCategory;
using CulinaryBlog.Application.Features.Categories.Commands.UpdateCategory;
using CulinaryBlog.Application.Features.Categories.Queries.GetCategories;
using CulinaryBlog.Application.Features.Categories.Queries.GetCategoryBySlug;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryBlog.WebApi.Endpoints;

public static class CategoryEndpoints
{
	public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
	{
		// Route Group: tất cả endpoints trong group có prefix /api/v1/categories
		// WithTags: nhóm các endpoint trong Scalar UI
		var group = app.MapGroup("/api/v1/categories")
			.WithTags("Categories");

		// ── GET /api/v1/categories ──────────────────────────────────────
		group.MapGet("/", async (
				[AsParameters] GetCategoriesQuery query,
				ISender sender,
				CancellationToken cancellationToken) =>
			{
				var result = await sender.Send(query, cancellationToken);
				return Results.Ok(result);
			})
			.WithName("GetCategories")
			.WithSummary("Lấy danh sách danh mục (có phân trang)")
			.WithDescription(
				"Hỗ trợ tìm kiếm theo tên và mô tả, sắp xếp và phân trang. " +
				"Ví dụ: ?search=abc&sortBy=name&pageNumber=1&pageSize=10")
			.Produces<PaginatedResult<CategoryDto>>(200);

		// ── GET /api/v1/categories/{slug} ───────────────────────────────
		group.MapGet("/{slug}", async (string slug, ISender sender, CancellationToken cancellationToken) =>
			{
				var result = await sender.Send(new GetCategoryBySlugQuery(slug), cancellationToken);
				return result == null ? Results.NotFound() : Results.Ok(result);
			})
			.WithName("GetCategoryBySlug")
			.WithSummary("Lấy thông tin chi tiết danh mục theo slug")
			.WithDescription("Trả về thông tin chi tiết của một danh mục dựa trên slug của nó.")
			.Produces<CategoryDto>(200)
			.ProducesProblem(404);

		// ── POST /api/v1/categories ─────────────────────────────────────
		group.MapPost("/", async (
				[FromBody] CreateCategoryCommand command, 
				ISender sender, 
				CancellationToken cancellationToken) =>
			{
				var result = await sender.Send(command, cancellationToken);

				// 201 Created + Location header trỏ đến resource mới
				return Results.Created($"/api/v1/categories/{result.Slug}", result);
			})
			.WithName("CreateCategory")
			.WithSummary("Tạo một danh mục mới")
			.WithDescription("Tạo một danh mục mới với thông tin được cung cấp.")
			.Produces<CategoryDto>(201)
			.ProducesProblem(409);          // Conflict nếu đã tồn tại danh mục với tên hoặc slug giống nhau

		// ── PUT /api/v1/categories/{id} ─────────────────────────────────
		group.MapPut("/{id:guid}", async (
				Guid id,
				[FromBody] UpdateCategoryRequest request,
				ISender sender,
				CancellationToken cancellationToken) =>
			{
				var command = new UpdateCategoryCommand(
					id, request.Name, request.Description, request.ImageUrl);
				var result = await sender.Send(command, cancellationToken);
				return Results.Ok(result);
			})
			.WithName("UpdateCategory")
			.WithSummary("Cập nhật thông tin danh mục")
			.WithDescription("Cập nhật thông tin của một danh mục dựa trên ID của nó.")
			.Produces<CategoryDto>(200)
			.ProducesProblem(404);

		// ── DELETE /api/v1/categories/{id} ──────────────────────────────
		group.MapDelete("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
			{
				await sender.Send(new DeleteCategoryCommand(id), cancellationToken);
				return Results.NoContent();     // 204 No Content nếu xóa thành công, không có body
			})
			.WithName("DeleteCategory")
			.WithSummary("Xóa danh mục (chỉ khi không còn Recipe liên kết)")
			.WithDescription("Xóa một danh mục dựa trên ID của nó (chỉ khi không còn Recipe liên kết).")
			.Produces(204)
			.ProducesProblem(404)
			.ProducesProblem(400);      // Bad Request nếu còn Recipe liên kết

		return app;
	}

	private record UpdateCategoryRequest(
		string Name,
		string? Description,
		string? ImageUrl
	);
}