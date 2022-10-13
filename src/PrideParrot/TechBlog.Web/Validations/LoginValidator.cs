using FluentValidation;
using TechBlog.Web.Models;

namespace TechBlog.Web.Validations;

public class LoginValidator : AbstractValidator<LoginModel>
{
	public LoginValidator()
	{
		RuleFor(x => x.UserName)
			.NotEmpty()
			.WithMessage("Username is required");

		RuleFor(x => x.Password)
			.NotEmpty()
			.WithMessage("Username is required");
		
		RuleFor(x => x.CaptchaToken)
			.NotEmpty()
			.WithMessage("Invalid captcha token");
	}
}