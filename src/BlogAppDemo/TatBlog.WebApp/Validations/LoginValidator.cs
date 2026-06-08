//using FluentValidation;
//using TatBlog.WebApp.Areas.Admin.Models;

//namespace TatBlog.WebApp.Validations;

//public class LoginValidator : AbstractValidator<LoginModel>
//{
//	public LoginValidator()
//	{
//		RuleFor(x => x.UserName)
//			.NotEmpty()
//			.WithMessage("Username is required");

//		RuleFor(x => x.Password)
//			.NotEmpty()
//			.WithMessage("Password is required");
//	}
//}