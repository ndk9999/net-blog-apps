using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations;

public class LoginModelValidator : AbstractValidator<LoginModel>
{
	public LoginModelValidator()
	{
		RuleFor(x => x.Username)
			.NotEmpty()
			.WithMessage("Username is required");

		RuleFor(x => x.Password)
			.NotEmpty()
			.WithMessage("Password is required")
			.MinimumLength(6)
			.WithMessage("Password must have at least 6 characters");
	}
}