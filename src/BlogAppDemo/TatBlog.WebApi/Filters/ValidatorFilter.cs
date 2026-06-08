using System.Net;
using FluentValidation;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Filters;

public class ValidatorFilter<T> : IEndpointFilter where T : class
{
	private readonly IValidator<T> _validator;

	public ValidatorFilter(IValidator<T> validator)
	{
		_validator = validator;
	}


	public async ValueTask<object> InvokeAsync(
		EndpointFilterInvocationContext context, 
		EndpointFilterDelegate next)
	{
		var model = context.Arguments
			.SingleOrDefault(x => x?.GetType() == typeof(T)) as T;

		if (model == null)
		{
			//return Results.BadRequest();
			return Results.Ok(ApiResponse.Fail(
				HttpStatusCode.BadRequest, "Could not create model object"));
		}

		var validationResult = await _validator.ValidateAsync(model);

		if (!validationResult.IsValid)
		{
			//return Results.BadRequest(validationResult.Errors.ToResponse());
			return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.BadRequest, validationResult));
		}

		return await next(context);
	}
}