using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TatBlog.Services.Blogs;
using TatBlog.ZooApp.Models;

namespace TatBlog.ZooApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IBlogRepository _blogRepository;

		public HomeController(ILogger<HomeController> logger, IBlogRepository blogRepository)
		{
			_logger = logger;
			_blogRepository = blogRepository;
		}

		public async Task<IActionResult> Index(int page = 10)
		{
			var categories = await _blogRepository.GetCategoriesAsync();
			ViewBag.PageNumber = page;

			return View(categories);
		}

		public IActionResult Login()
		{
			return View(new SignInModel());
		}

		[HttpPost]
		public IActionResult SignIn(SignInModel model, [Bind(Prefix = "Login")] RememberModel rm, [FromHeader] string accept)
		{
			if (!ModelState.IsValid)
			{
				var messages = ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));
				TempData["error"] = "Invalid data. " + string.Join(", ", messages);
				return RedirectToAction(nameof(Index));
			}

			TempData["name"] = model.Username;
			TempData["password"] = model.Password;

			return RedirectToAction(nameof(ShowData));
		}

		public IActionResult ShowData()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}