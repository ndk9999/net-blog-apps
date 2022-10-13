using FluentValidation;
using TechBlog.Services.Blogs;
using TechBlog.Web.Models;
using TechBlog.Web.Validations.CustomValidators;

namespace TechBlog.Web.Validations;

public class PostValidator : AbstractValidator<PostEditModel>
{
	private readonly IBlogRepository _blogRepository;

	public PostValidator(IBlogRepository blogRepository)
	{
		_blogRepository = blogRepository;

		RuleFor(x => x.Title)
			.NotEmpty()
			.MaximumLength(500);

		RuleFor(x => x.ShortDescription)
			.NotEmpty();

		RuleFor(x => x.Description)
			.NotEmpty();

		RuleFor(x => x.Meta)
			.NotEmpty()
			.MaximumLength(1000);

		RuleFor(x => x.UrlSlug)
			.NotEmpty()
			.MaximumLength(1000)
			.NotUsedByOtherPost(blogRepository);

		RuleFor(x => x.CategoryId)
			.NotEmpty()
			.WithMessage("You must select a category");

		RuleFor(x => x.SelectedTags)
			.Must(HasAtLeastOneTag)
			.WithMessage("You must provide at least one tag");

		When(x => x.Id <= 0, () =>
		{
			RuleFor(x => x.ImageFile)
				.Must(x => x is {Length: > 0})
				.WithMessage("You must set an image for the post");
		})
		.Otherwise(() =>
		{
			RuleFor(x => x.ImageFile)
				.MustAsync(SetImageIfNotExist)
				.WithMessage("You must set an image for the post");
		});
	}

	private bool HasAtLeastOneTag(PostEditModel postModel, string selectedTags)
	{
		return postModel.GetSelectedTags().Any();
	}

	private async Task<bool> SetImageIfNotExist(
		PostEditModel postModel,
		IFormFile imageFile,
		CancellationToken cancellationToken)
	{
		var post = await _blogRepository.GetPostByIdAsync(postModel.Id, false, cancellationToken);

		if (!string.IsNullOrWhiteSpace(post?.ImageUrl)) return true;

		return imageFile is { Length: >0 };
	}
}