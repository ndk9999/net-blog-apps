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
using TechBlog.Core.Constants;
using TechBlog.Services.Blogs;
using TechBlog.Web.Extensions;

namespace TechBlog.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ICaptchaProvider _captchaProvider;
		private readonly IFluentEmail _fluentEmail;
		private readonly IWebHostEnvironment _environment;
		private readonly IFeedProvider _feedProvider;
		private readonly ISubscriberRepository _subscriberRepository;
		private readonly MailingSettings _mailingSettings;

		public HomeController(
			ILogger<HomeController> logger, 
			ICaptchaProvider captchaProvider, 
			IFluentEmail fluentEmail, 
			IWebHostEnvironment environment, 
			IFeedProvider feedProvider,
			ISubscriberRepository subscriberRepository,
			IOptions<MailingSettings> mailingSettings)
		{
			_logger = logger;
			_captchaProvider = captchaProvider;
			_fluentEmail = fluentEmail;
			_environment = environment;
			_feedProvider = feedProvider;
			_subscriberRepository = subscriberRepository;
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

			var templatePath = _environment.GetEmailTemplateFullPath(Default.EmailTemplates.ContactUs);

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
		public async Task<IActionResult> Subscribe(SubscribeModel model)
		{
			// Notify invalid email address
			if (!ModelState.IsValid)
			{
				return Json(AjaxResponse.Error(
					$"You provided invalid email address '{model.Email}'."));
			}

			// Check and add new subscriber
			var subscriber = await _subscriberRepository.SubscribeAsync(model.Email);

			// Notify email blocked message
			if (subscriber.UnsubscribedDate != null && !subscriber.Voluntary)
			{
				return Json(AjaxResponse.Error(
					$"Your email '{model.Email}' is blocked by administrator"));
			}

			// Send welcome email
			var templatePath = _environment.GetEmailTemplateFullPath(Default.EmailTemplates.WelcomeSubscriber);

			await _fluentEmail
				.To(model.Email)
				.Subject("[DO NOT REPLY] Thanks For Subscribing To TechBlog")
				.UsingTemplateFromFile(templatePath, new
				{
					UnsubscribeLink = Url.Action(
						"Unsubscribe", "Home", new {model.Email}, Request.Scheme)
				})
				.SendAsync();

			return Json(AjaxResponse.Ok(model.Email));
		}

		public async Task<IActionResult> Unsubscribe(string email)
		{
			var subscriber = string.IsNullOrWhiteSpace(email)
				? null
				: await _subscriberRepository.GetSubscriberByEmailAsync(email);

			var model = new UnsubscribeModel()
			{
				Email = email,
				Message = subscriber == null
					? $"Your email '{email}' does not exist in our system"
					: subscriber.UnsubscribedDate != null
						? "You already unsubscribed from our system"
						: null
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Unsubscribe(UnsubscribeModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			await _subscriberRepository.UnsubscribeAsync(
				model.Email, model.Reason, true);

			return RedirectToAction("Unsubscribed", "Home", new {model.Email});
		}

		[Route("unsubscribed")]
		public IActionResult Unsubscribed(string email)
		{
			ViewBag.EmailAddress = email;
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