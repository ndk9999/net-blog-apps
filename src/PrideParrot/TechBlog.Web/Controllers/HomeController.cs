using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FluentEmail.Core;
using Microsoft.Extensions.Options;
using TechBlog.Core.Settings;
using TechBlog.Services.Rss;
using TechBlog.Services.Security;
using TechBlog.Web.Models;
using System.IO;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace TechBlog.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ICaptchaProvider _captchaProvider;
		private readonly IFluentEmail _fluentEmail;
		private readonly IWebHostEnvironment _environment;
		private readonly IFeedProvider _feedProvider;
		private readonly MailingSettings _mailingSettings;

		public HomeController(
			ILogger<HomeController> logger, 
			ICaptchaProvider captchaProvider, 
			IFluentEmail fluentEmail, 
			IWebHostEnvironment environment, 
			IFeedProvider feedProvider, 
			IOptions<MailingSettings> mailingSettings)
		{
			_logger = logger;
			_captchaProvider = captchaProvider;
			_fluentEmail = fluentEmail;
			_environment = environment;
			_feedProvider = feedProvider;
			_mailingSettings = mailingSettings.Value;
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
		public async Task<IActionResult> Contact(ContactFormModel model,
			[FromServices] IValidator<ContactFormModel> validator)
		{
			var validationResult = await validator.ValidateAsync(model);

			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(ModelState);
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var templatePath = Path.Combine(
				_environment.WebRootPath, "templates", "emails", "contact-us.html");

			await _fluentEmail
				.To(_mailingSettings.ReceiverAddress)
				.ReplyTo(model.Email)
				.Subject(model.Subject)
				.UsingTemplateFromFile(templatePath, model)
				.SendAsync();

			TempData["ThankYou"] = "Thank you for contact us. We will reply you soon.";

			return RedirectToAction("Contact");
		}

		[HttpPost]
		public IActionResult Subscribe(SubscribeFormModel model)
		{
			return View();
		}

		[ResponseCache(Duration = 3600)]
		public async Task<IActionResult> Rss()
		{
			var rssBuffer = await _feedProvider.CreateAsync();
			
			return File(rssBuffer, "application/rss+xml; charset=utf-8");
		}

		[Route("/error/{code:int}")]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error(int? code)
		{
			return View(new ErrorViewModel
			{
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				Message = code > 0 ? $"Error code: {code}" : "Unhandled Exception"
			});
		}
	}
}