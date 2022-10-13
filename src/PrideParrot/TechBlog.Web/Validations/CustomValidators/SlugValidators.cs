using FluentValidation;
using TechBlog.Services.Blogs;
using TechBlog.Web.Models;

namespace TechBlog.Web.Validations.CustomValidators;

public static class SlugValidators
{
	public static IRuleBuilderOptions<CategoryEditModel, string> NotUsedByOtherCategory(
		this IRuleBuilder<CategoryEditModel, string> ruleBuilder, IBlogRepository blogRepository)
	{
		return ruleBuilder
			.MustAsync(async (categoryModel, slug, cancellationToken) => 
				!await blogRepository.IsCategorySlugExistedAsync(categoryModel.Id, slug, cancellationToken))
			.WithMessage("Slug '{PropertyValue}' is already in use");
	}

	public static IRuleBuilderOptions<PostEditModel, string> NotUsedByOtherPost(
		this IRuleBuilder<PostEditModel, string> ruleBuilder, IBlogRepository blogRepository)
	{
		return ruleBuilder
			.MustAsync(async (postModel, slug, cancellationToken) =>
				!await blogRepository.IsPostSlugExistedAsync(postModel.Id, slug, cancellationToken))
			.WithMessage("Slug '{PropertyValue}' is already in use");
	}
}