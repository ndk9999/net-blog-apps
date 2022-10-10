using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechBlog.Web.Models;
using TechBlog.Web.Providers;

namespace TechBlog.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ICaptchaProvider _captchaProvider;

		public HomeController(
			ILogger<HomeController> logger, 
			ICaptchaProvider captchaProvider)
		{
			_logger = logger;
			_captchaProvider = captchaProvider;
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
		public async Task<IActionResult> Contact(ContactFormModel model)
		{
			if (!await _captchaProvider.VerifyAsync(model))
			{
				ModelState.AddModelError("", "Invalid captcha token");
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			TempData["ThankYou"] = "Thank you for contact us. We will reply you soon.";

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