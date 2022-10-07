using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechBlog.Web.Models;

namespace TechBlog.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult About()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Contact()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Contact(ContactFormModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			return RedirectToAction("Contact");
		}

		[HttpPost]
		public IActionResult Subscribe(SubscribeFormModel model)
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				Message = "Unhandled Exception"
			});
		}
	}
}