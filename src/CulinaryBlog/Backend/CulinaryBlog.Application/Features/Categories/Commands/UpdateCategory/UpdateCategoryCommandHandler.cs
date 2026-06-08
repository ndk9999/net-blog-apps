using CulinaryBlog.Application.Contracts.Persistence;
using CulinaryBlog.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaryBlog.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler(IApplicationDbContext dbContext)
	: IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
	public async Task<CategoryDto> Handle(
		UpdateCategoryCommand request,
		CancellationToken cancellationToken)
	{
		// Tìm danh mục cần cập nhật trong cơ sở dữ liệu
		var category = await dbContext.Categories
			.Include(x => x.Recipes)
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		// Kiểm tra xem danh mục có tồn tại hay không
		if (category == null)
			throw new KeyNotFoundException($"Không tìm thấy danh mục với Id = {request.Id}");

		// Cập nhật thông tin danh mục
		category.Update(request.Name, request.Description, request.ImageUrl);
		await dbContext.SaveChangesAsync(cancellationToken);

		return new CategoryDto(
			category.Id,
			category.Name,
			category.Slug,
			category.Description,
			category.ImageUrl,
			category.Recipes.Count,
			category.CreatedAt
		);
	}
}