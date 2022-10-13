using System.Security.Policy;
using FluentEmail.Core;
using Microsoft.Extensions.Options;
using NuGet.Protocol.Plugins;
using TechBlog.Core.Constants;
using TechBlog.Core.DTO;
using TechBlog.Core.Entities;
using TechBlog.Core.Settings;
using TechBlog.Services.Blogs;
using TechBlog.Web.Extensions;

namespace TechBlog.Web.BackgroundTasks;

public class SendNewsletterTask : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<SendNewsletterTask> _logger;
	private readonly IWebHostEnvironment _environment;
	private readonly INewsletterQueue _newsletterQueue;
	private readonly RssSettings _rssSettings;

	public SendNewsletterTask(
		IServiceProvider serviceProvider, 
		IWebHostEnvironment environment, 
		INewsletterQueue newsletterQueue, 
		ILogger<SendNewsletterTask> logger, 
		IOptions<RssSettings> rssSettings)
	{
		_serviceProvider = serviceProvider;
		_environment = environment;
		_newsletterQueue = newsletterQueue;
		_logger = logger;
		_rssSettings = rssSettings.Value;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using var scope = _serviceProvider.CreateScope();
		var subscriberRepository = scope.ServiceProvider.GetRequiredService<ISubscriberRepository>();
		var fluentEmail = scope.ServiceProvider.GetRequiredService<IFluentEmail>();

		while (!stoppingToken.IsCancellationRequested)
		{
			while (_newsletterQueue.TryDequeue(out var post))
			{
				await SendNewslettersAsync(fluentEmail, subscriberRepository, post, stoppingToken);
				await Task.Delay(60 * 1000, stoppingToken);
			}

			await Task.Delay(5 * 60 * 1000, stoppingToken);
		}

		_logger.LogWarning("Newsletter task is stopped");
	}

	private async Task SendNewslettersAsync(
		IFluentEmail fluentEmail, 
		ISubscriberRepository subscriberRepository,
		Post post,
		CancellationToken cancellationToken)
	{
		const int pageSize = 20;

		try
		{
			var receivers = await subscriberRepository
				.GetNewsletterReceiversAsync(1, pageSize, cancellationToken);

			while (receivers.Count > 0)
			{
				foreach (var subscriber in receivers)
				{
					await SendNewsletterEmailAsync(fluentEmail, post, subscriber, cancellationToken);
				}

				if (receivers.HasNextPage)
				{
					receivers = await subscriberRepository
						.GetNewsletterReceiversAsync(receivers.PageNumber + 1, pageSize, cancellationToken);
				}
				else
				{
					break;
				}
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, ex.Message);
		}
	}

	private async Task SendNewsletterEmailAsync(
		IFluentEmail fluentEmail,
		Post post,
		Subscriber receiver,
		CancellationToken cancellationToken)
	{
		try
		{
			var templatePath = _environment
				.GetEmailTemplateFullPath(Default.EmailTemplates.Newsletter);

			await fluentEmail
				.To(receiver.Email)
				.Subject("TechBlog Newsletter")
				.UsingTemplateFromFile(templatePath, new
				{
					Title = post.Title,
					ShortDescription = post.ShortDescription,
					PostedDate = post.PostedDate.ToString("dd/MM/yyyy HH:mm"),
					ImageUrl = $"{_rssSettings.Domain}/{post.ImageUrl}",
					Link = GetPostLink(post)
				})
				.SendAsync(cancellationToken);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Cannot send newsletter to {receiver.Email}");
		}
	}

	private string GetPostLink(Post post)
	{
		return string.Format("{0}/post/{1}/{2}/{3}/{4}/",
			_rssSettings.Domain,
			post.PostedDate.Year,
			post.PostedDate.Month,
			post.PostedDate.Day,
			post.UrlSlug);
	}
}