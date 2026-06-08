using DeThiThu.Contexts;
using DeThiThu.Entities;
using DeThiThu.Models;
using DeThiThu.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CandyContext>(opt =>
	opt.UseSqlServer(builder.Configuration.GetConnectionString(nameof(CandyContext))));

builder.Services.AddScoped<ICandyService, CandyService>();
builder.Services.AddScoped<ISubscriberService, SubscriberService>();
builder.Services.AddScoped<IDataSeeder, DataSeeder>();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(opts => opts.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapGet("/candies", async ([FromServices] ICandyService service, [AsParameters] CandyFilterModel model) =>
{
	var candies = await service.GetCandiesAsync(
		model.Name, model.CategoryId, model.CategoryName,
		model.MinPrice, model.MaxPrice);

	return candies;
})
.WithName("GetCandies")
.Produces<Candy>()
.WithOpenApi();

app.MapPost("/categories", async ([FromServices] ICandyService service, [FromBody] CategoryEditModel model, IValidator<CategoryEditModel> validator) =>
{
	var validationResult = await validator.ValidateAsync(model);

	if (!validationResult.IsValid)
	{
		var errors = validationResult.ToDictionary();
		return Results.ValidationProblem(errors);
	}

	var category = await service.AddOrUpdateCategoryAsync(
		model.Name, model.ShowOnMenu, model.Id);

	return Results.Ok(category);
})
.WithName("AddCategory")
.Produces<Category>()
.WithOpenApi();


app.MapGet("/subscribers", async (
		[FromServices] ISubscriberService service, 
		[AsParameters] SubscriberFilterModel model) =>
{
	var subscribers = await service.GetSubscribersAsync(model.SubscribedDate, model.N);
	return Results.Ok(subscribers);
})
.WithName("GetSubscribers")
.Produces<List<Subscriber>>()
.WithOpenApi();

app.MapPost("/subscribers", async (
		[FromServices] ISubscriberService service, 
		SubscribeModel model, 
		IValidator<SubscribeModel> validator) =>
{
	var validationResult = validator.Validate(model);

	if (!validationResult.IsValid)
	{
		return Results.BadRequest("Địa chỉ email không hợp lệ");
	}

	if (!await service.AddSubscriberAsync(model.Email))
	{
		return Results.Conflict("Email đã tồn tại");
	}

	return Results.Ok("Đã đăng ký thành công");
})
.WithName("AddSubscriber")
.Produces<string>(200)
.Produces<string>(400)
.Produces<string>(409)
.WithOpenApi();

await using (var scope = app.Services.CreateAsyncScope())
{
	var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
	await seeder.ImportAsync();
}

app.Run();