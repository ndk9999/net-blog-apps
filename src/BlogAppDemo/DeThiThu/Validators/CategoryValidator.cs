using DeThiThu.Models;
using FluentValidation;

namespace DeThiThu.Validators;

public class CategoryValidator : AbstractValidator<CategoryEditModel>
{
	public CategoryValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.WithMessage("Tên danh mục không được để trống");
	}
}