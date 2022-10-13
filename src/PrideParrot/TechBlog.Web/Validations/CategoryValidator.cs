using FluentValidation;
using TechBlog.Services.Blogs;
using TechBlog.Web.Models;

namespace TechBlog.Web.Validations;

public class CategoryValidator : AbstractValidator<CategoryEditModel>
{
	private readonly IBlogRepository _blogRepository;

	public CategoryValidator(IBlogRepository blogRepository)
	{
		_blogRepository = blogRepository;

		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(50);

		RuleFor(x => x.UrlSlug)
			.NotEmpty()
			.MaximumLength(50)
			.MustAsync(NotUsedForOtherCategory)
			.WithMessage("Slug '{PropertyValue}' is already in use");

		RuleFor(x => x.Description)
			.MaximumLength(500);
	}

	private async Task<bool> NotUsedForOtherCategory(
		CategoryEditModel categoryModel, 
		string slug,
		CancellationToken cancellationToken)
	{
		return !await _blogRepository.IsCategorySlugExistedAsync(
			categoryModel.Id, slug, cancellationToken);
	}
}