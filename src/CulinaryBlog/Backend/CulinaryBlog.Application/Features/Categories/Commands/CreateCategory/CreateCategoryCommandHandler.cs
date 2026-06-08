using CulinaryBlog.Application.Contracts.Persistence;
using CulinaryBlog.Application.DTOs;
using CulinaryBlog.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaryBlog.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler(IApplicationDbContext dbContext)
	: IRequestHandler<CreateCategoryCommand, CategoryDto>
{
	public async Task<CategoryDto> Handle(
		CreateCategoryCommand request,
		CancellationToken cancellationToken)
	{
		// Kiểm tra Slug đã tồn tại hay chưa trước khi tạo mới
		var slug = request.Name.ToSlug();
		
		if (await dbContext.Categories.AnyAsync(c => c.Slug == slug, cancellationToken: cancellationToken))
		{
			throw new InvalidOperationException($"Danh mục với slug '{slug}' đã tồn tại.");
		}

		// Tạo mới danh mục qua domain factory method
		var category = Domain.Entities.Category.Create(
			request.Name,
			request.Description,
			request.ImageUrl);

		// Lưu danh mục mới vào database
		dbContext.Categories.Add(category);
		await dbContext.SaveChangesAsync(cancellationToken);

		// Trả về DTO sau khi đã lưu thành công
		// Map entity sang DTO. RecipeCount sẽ được set là 0 vì mới tạo, chưa có công thức nào thuộc danh mục này.
		return new CategoryDto(
			category.Id,
			category.Name,
			category.Slug,
			category.Description,
			category.ImageUrl,
			RecipeCount: 0,
			category.CreatedAt
		);
	}
}