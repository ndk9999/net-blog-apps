using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.ZooApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<BlogDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IDataSeeder, DataSeeder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.Use(async (context, next) =>
//{
//	app.Logger.LogInformation("IP: {IpAddress} - Path: {Url}",
//		context.Connection.RemoteIpAddress?.ToString(), context.Request.Path);

//	await next();
//});
app.UseMiddleware<UserTrackingMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.MapGet("/hello/{name?}", (string? name) => $"Hello {name ?? "world"}!");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
	var seeder = scope.ServiceProvider
		.GetRequiredService<IDataSeeder>();
	seeder.Initialize();
}

app.Run();
