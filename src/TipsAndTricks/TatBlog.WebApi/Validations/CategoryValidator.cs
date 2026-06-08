//using FluentValidation;
//using TatBlog.WebApi.Models;

//namespace TatBlog.WebApi.Validations;

//public class CategoryValidator : AbstractValidator<CategoryEditModel>
//{
//	public CategoryValidator()
//	{
//		RuleFor(a => a.Name)
//			.NotEmpty()
//			.WithMessage("Tên chuyên mục không được để trống")
//			.MaximumLength(50)
//			.WithMessage("Tên chuyên mục tối đa 50 ký tự");

//		RuleFor(a => a.UrlSlug)
//			.NotEmpty()
//			.WithMessage("UrlSlug không được để trống")
//			.MaximumLength(50)
//			.WithMessage("UrlSlug tối đa 50 ký tự");

//		RuleFor(a => a.Description)
//			.NotEmpty()
//			.WithMessage("Mô tả không được để trống")
//			.MaximumLength(500)
//			.WithMessage("Mô tả chứa tối đa 500 ký tự");
//	}
//}