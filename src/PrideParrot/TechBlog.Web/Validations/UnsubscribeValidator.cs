using FluentValidation;
using TechBlog.Web.Models;

namespace TechBlog.Web.Validations;

public class UnsubscribeValidator : AbstractValidator<UnsubscribeModel>
{
	public UnsubscribeValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.WithMessage("Username is required")
			.EmailAddress();

		RuleFor(x => x.Reason)
			.NotEmpty()
			.WithMessage("Please let us know the reason");
	}
}