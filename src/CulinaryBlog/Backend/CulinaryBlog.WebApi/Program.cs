using CulinaryBlog.Application;
using CulinaryBlog.Infrastructure;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
	options.AddDocumentTransformer((document, context, ct) =>
	{
		document.Info = new OpenApiInfo
		{
			Title = "Culinary Blog API",
			Version = "v1",
			Description = "API for managing culinary blog posts, recipes, and user interactions.",
		};
		return Task.CompletedTask;
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	// OpenAPI spec endpoint: /openapi/v1.json
	app.MapOpenApi();

	// Scalar UI: /scalar/v1
	app.MapScalarApiReference(options =>
	{
		options
			.WithTitle("Culinary Blog API")
			.WithTheme(ScalarTheme.Purple)
			.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
	});
}

app.UseHttpsRedirection();

// Endpoints will be added here
//app.MapCategoryEndpoints();
//app.MapRecipeEndpoints();

app.Run();

// Partial class for TestContainers/WebApplicationFactory can access (Lab 5)
public partial class Program
{
}