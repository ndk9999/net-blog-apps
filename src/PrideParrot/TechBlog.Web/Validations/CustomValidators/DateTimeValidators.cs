using FluentValidation;

namespace TechBlog.Web.Validations.CustomValidators;

public static class DateTimeValidators
{
	public static IRuleBuilderOptions<T, DateTime> AfterSunrise<T>(
		IRuleBuilder<T, DateTime> ruleBuilder)
	{
		var sunrise = TimeOnly.MinValue.AddHours(6);

		return ruleBuilder
			.Must((objectRoot, dateTime, context) =>
			{
				var providedTime = TimeOnly.FromDateTime(dateTime);

				context.MessageFormatter.AppendArgument("Sunrise", sunrise);
				context.MessageFormatter.AppendArgument("ProvidedTime", providedTime);

				return providedTime > sunrise;
			})
			.WithMessage("{PropertyName} must be after sunrise {Sunrise}. You provided value {ProvidedTime}.");
	}
}