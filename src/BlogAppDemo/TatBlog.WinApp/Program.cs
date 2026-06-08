using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs;

IConfiguration configuration = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
	.Build();

var services = new ServiceCollection()
	.AddSingleton(configuration)
	.AddDbContext<BlogDbContext>(options => 
		options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
	.AddScoped<IBlogRepository, BlogRepository>();

var serviceProvider =  services.BuildServiceProvider();

var blog = serviceProvider.GetRequiredService<IBlogRepository>();

//var deleted = await blog.DeleteTagAsync(12);

//Console.WriteLine(deleted);

var testTag = new Tag()
{
	Id = 15,
	Name = "TEST 1",
	UrlSlug = "test-1",
	Description = "Test 1 Updated",
	Posts = new List<Post>()
};

var changed = await blog.CreateOrUpdateTagAsync(testTag);

Console.WriteLine(changed);