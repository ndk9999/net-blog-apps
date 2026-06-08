using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.BackgroundTasks;

public class TimeLogTask : BackgroundService
{
	private readonly ILogger<TimeLogTask> _logger;
	private readonly IServiceProvider _serviceProvider;
	private readonly IWebHostEnvironment _environment;

	public TimeLogTask(
		ILogger<TimeLogTask> logger, 
		IServiceProvider serviceProvider, 
		IWebHostEnvironment environment)
	{
		_logger = logger;
		_serviceProvider = serviceProvider;
		_environment = environment;
	}


	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			_logger.LogInformation("Current time is: {Time}", DateTime.Now);
			await Task.Delay(3000, stoppingToken);

			await Task.Delay(10000, stoppingToken);
		}
	}
}