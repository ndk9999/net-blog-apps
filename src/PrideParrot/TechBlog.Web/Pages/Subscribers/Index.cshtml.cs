using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechBlog.Core.Contracts;
using TechBlog.Core.Entities;
using TechBlog.Services.Blogs;
using TechBlog.Web.Models;

namespace TechBlog.Web.Pages.Subscribers
{
    public class IndexModel : PageModel, IPagingParams
    {
	    private readonly ILogger<IndexModel> _logger;
	    private readonly ISubscriberRepository _subscriberRepository;


        [BindProperty(SupportsGet = true)]
		[DisplayName("Keyword (email or note)")]
	    public string Keyword { get; set; }

	    [BindProperty(SupportsGet = true)]
		[DisplayName("Unsubscribed users only")]
		public bool Unsubscribed { get; set; }

	    [BindProperty(SupportsGet = true)]
	    [DisplayName("Involuntary only")]
		public bool Involuntary { get; set; }

		[BindProperty(SupportsGet = true)]
	    public int PageSize { get; set; } = 20;

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
		[DisplayName("Sort By")]
        public string SortColumn { get; set; } = "Email";

        [BindProperty(SupportsGet = true)]
		[DisplayName("Sort Order")]
        public string SortOrder { get; set; } = "ASC";

        public IPagedList<Subscriber> Subscribers { get; set; }

        public IEnumerable<SelectListItem> ColumnList { get; set; }

        public IEnumerable<SelectListItem> OrderList { get; set; }


        public IndexModel(
	        ILogger<IndexModel> logger, 
	        ISubscriberRepository subscriberRepository)
        {
	        _logger = logger;
	        _subscriberRepository = subscriberRepository;
        }

        public async Task OnGetAsync()
        {
	        Subscribers = await _subscriberRepository.SearchSubscribersAsync(
		        this, Keyword, Unsubscribed, Involuntary);

	        ColumnList = new[] {"Id", "Email", "SubscribedDate", "UnsubscribedDate"}
		        .Select(x => new SelectListItem(x, x))
		        .ToList();

	        OrderList = new List<SelectListItem>()
	        {
				new("Ascending", "ASC"),
				new("Descending", "DESC")
			};
		}

        public async Task<IActionResult> OnPostDeleteSubscriberAsync(int id)
        {
	        await _subscriberRepository.DeleteSubscriberAsync(id);

	        return new JsonResult(AjaxResponse.Ok(id));
        }
	}
}
