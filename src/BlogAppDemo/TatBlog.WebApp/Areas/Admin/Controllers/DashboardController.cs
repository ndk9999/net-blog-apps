using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Memory;

namespace TatBlog.WebApp.Areas.Admin.Controllers;

public class DashboardController : Controller
{
	private readonly ILogger<DashboardController> _logger;
	private readonly IMemoryCache _memoryCache;

	public DashboardController(ILogger<DashboardController> logger, IMemoryCache memoryCache)
	{
		_logger = logger;
		_memoryCache = memoryCache;
	}

	//[ResponseCache(Duration = 15, Location = ResponseCacheLocation.Any)]
	public IActionResult Index()
	{
		_logger.LogInformation("Request duoc xu ly boi action Index");

		// Đọc dữ liệu từ db
		ViewBag.CurrentTime = _memoryCache.GetOrCreate("CurrentTime", entry =>
		{
			entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15);
			return DateTime.Now;
		});

		return View();
	}
}