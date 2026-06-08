using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Mapsters;
using TatBlog.WebApi.Validations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
	.ConfigureCors()
	.ConfigureSettings()
	.ConfigureServices()
	.ConfigureSwaggerOpenApi()
	.ConfigureAuth()
	.ConfigureMapster()
	.ConfigureFluentValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.SetupRequestPipeline();

// Define endpoints & handlers
app.MapAuthorEndpoints()
	.MapCategoryEndpoints()
	.MapPostEndpoints()
	.MapTagEndpoints()
	.MapSubscriberEndpoints()
	.MapUserEndpoints();

app.Run();