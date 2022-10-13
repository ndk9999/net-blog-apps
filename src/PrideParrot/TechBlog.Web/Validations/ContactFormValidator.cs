using FluentValidation;
using TechBlog.Services.Security;
using TechBlog.Web.Models;

namespace TechBlog.Web.Validations;

public class ContactFormValidator : AbstractValidator<ContactFormModel>
{
	private readonly ICaptchaProvider _captchaProvider;

	public ContactFormValidator(ICaptchaProvider captchaProvider)
	{
		_captchaProvider = captchaProvider;

		RuleFor(x => x.FullName).NotEmpty();
		RuleFor(x => x.Email).NotEmpty().EmailAddress();
		RuleFor(x => x.Subject).NotEmpty();
		RuleFor(x => x.Message).NotEmpty();
		RuleFor(x => x.CaptchaToken)
			.MustAsync(BeValidCaptchaToken);
	}

	private async Task<bool> BeValidCaptchaToken(
		ContactFormModel contactModel, 
		string captchaToken, 
		CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(captchaToken)) return false;

		return await _captchaProvider.VerifyAsync(contactModel);
	}
}