using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using TechBlog.Web.Mapsters;
using TechBlog.Web.Models;

namespace TechBlog.Web.Validations;

public static class FluentValidationDependencyInjection
{
	public static WebApplicationBuilder ConfigureFluentValidation(this WebApplicationBuilder builder)
	{
		// Manual register validators
		//builder.Services.AddScoped<IValidator<CategoryEditModel>, CategoryValidator>();

		// Enable client-side integration
		builder.Services.AddFluentValidationClientsideAdapters();

		// Scan and register all validators in given assembly
		builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		return builder;
	}
}