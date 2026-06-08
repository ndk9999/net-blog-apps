using CulinaryBlog.Application.Contracts.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CulinaryBlog.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(IApplicationDbContext dbContext)
	: IRequestHandler<DeleteCategoryCommand, Unit>
{
	public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
	{
		// Tìm danh mục cần cập nhật trong cơ sở dữ liệu
		var category = await dbContext.Categories
			.Include(x => x.Recipes)
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		// Kiểm tra xem danh mục có tồn tại hay không
		if (category == null)
			throw new KeyNotFoundException($"Không tìm thấy danh mục với Id = {request.Id}");

		if (category.Recipes.Any())
			throw new InvalidOperationException(
				$"Không thể xóa danh mục '#{request.Id}: {category.Name}' " +
				$"vì vẫn còn {category.Recipes.Count} công thức liên kết.");

		dbContext.Categories.Remove(category);
		await dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}