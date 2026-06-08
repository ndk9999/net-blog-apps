using System.Net;
using TatBlog.Core.Entities;
using TatBlog.Services.Security;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Extensions;

public static class UserEndpoints
{
	public static WebApplication MapUserEndpoints(this WebApplication app)
	{
		var routeGroupBuilder = app.MapGroup("/api/auth");

		routeGroupBuilder.MapPost("/login", SignIn)
			.AddEndpointFilter<ValidatorFilter<LoginModel>>()
			.WithName("UserLogin")
			.Produces<ApiResponse<LoginResult>>();
		
		return app;
	}

	private static async Task<IResult> SignIn(
		LoginModel model,
		IJwtTokenGenerator tokenGenerator)
	{
		if (!"admin".Equals(model.Username, StringComparison.InvariantCultureIgnoreCase) &&
		    !"123456".Equals(model.Password, StringComparison.InvariantCultureIgnoreCase))
		{
			return Results.Ok(ApiResponse.Fail(
				HttpStatusCode.BadRequest, "Invalid username or password"));
		}

		var token = tokenGenerator.GenerateToken(model.Username);

		var loginResult = new LoginResult(
			1, model.Username, $"{model.Username}@gmail.com", model.Username, token);

		return Results.Ok(ApiResponse.Success(loginResult));
	}

}