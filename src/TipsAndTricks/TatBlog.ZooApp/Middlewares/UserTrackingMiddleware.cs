namespace TatBlog.ZooApp.Middlewares;

public class UserTrackingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<UserTrackingMiddleware> _logger;

	public UserTrackingMiddleware(RequestDelegate next, ILogger<UserTrackingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task Invoke(HttpContext context)
	{
		_logger.LogInformation("{Time:yyyy-MM-dd HH:mm:ss} - IP: {IpAddress} - Path: {Url}",
			DateTime.Now,
			context.Connection.RemoteIpAddress?.ToString(), 
			context.Request.Path);

		await _next(context);
	}
}