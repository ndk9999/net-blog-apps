using MediatR;

namespace CulinaryBlog.Application.Features.Categories.Commands.DeleteCategory;

// Unit = MediatR built-in type cho "void" (không trả về data)
public record DeleteCategoryCommand(Guid Id) : IRequest<Unit>;