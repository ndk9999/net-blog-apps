using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TechBlog.Services.Blogs;
using TechBlog.Web.Extensions;
using TechBlog.Web.Models;

namespace TechBlog.Web.Pages.Subscribers
{
    public class BlockModel : PageModel
    {
	    private readonly ILogger<IndexModel> _logger;
	    private readonly ISubscriberRepository _subscriberRepository;


        [BindProperty(SupportsGet = true)]
	    public int Id { get; set; }

		[BindProperty]
	    public BlockSubscriberModel BanModel { get; set; }

		[TempData]
		public string Alert { get; set; }

        public BlockModel(
	        ILogger<IndexModel> logger, 
	        ISubscriberRepository subscriberRepository)
        {
	        _logger = logger;
	        _subscriberRepository = subscriberRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
	        var subscriber = await _subscriberRepository.GetSubscriberByIdAsync(Id);

	        if (subscriber == null || (subscriber.UnsubscribedDate != null && !subscriber.Voluntary))
	        {
		        Alert = AlertMessage.Error("Subscriber not found or was already blocked").AsJson();
				return RedirectToPage("/subscribers/index");
	        }

	        BanModel = new BlockSubscriberModel()
	        {
		        Id = subscriber.Id,
		        Email = subscriber.Email,
		        BlockedReason = subscriber.UnsubscribedReason,
				Involuntary = false,
		        Notes = subscriber.Notes
	        };

	        return Page();
        }

        public async Task<IActionResult> OnPostAsync(
			[FromServices] IValidator<BlockSubscriberModel> validator)
        {
	        var validationResult = await validator.ValidateAsync(BanModel);
			validationResult.AddToModelState(ModelState);

			if (!ModelState.IsValid)
			{
				return Page();
			}

			await _subscriberRepository.BlockSubscriberAsync(
				BanModel.Id, BanModel.BlockedReason, BanModel.Notes, BanModel.Involuntary);

			Alert = AlertMessage.Success($"Subscriber {BanModel.Email} was already blocked").AsJson();

			return RedirectToPage("/subscribers/index");
        }
	}
}
