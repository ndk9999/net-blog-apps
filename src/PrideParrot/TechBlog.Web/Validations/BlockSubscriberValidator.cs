using FluentValidation;
using TechBlog.Web.Models;

namespace TechBlog.Web.Validations;

public class BlockSubscriberValidator : AbstractValidator<BlockSubscriberModel>
{
	public BlockSubscriberValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.Email).NotEmpty().EmailAddress();
		RuleFor(x => x.BlockedReason).NotEmpty();
		RuleFor(x => x.Notes).NotEmpty();
	}
}