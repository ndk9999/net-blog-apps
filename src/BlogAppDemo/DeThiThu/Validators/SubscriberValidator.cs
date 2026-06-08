using DeThiThu.Models;
using FluentValidation;

namespace DeThiThu.Validators;

public class SubscriberValidator : AbstractValidator<SubscribeModel>
{
	public SubscriberValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
				.WithMessage("Địa chỉ email không được để trống")
			.EmailAddress()
				.WithMessage("Địa chỉ email không hợp lệ");
	}
}