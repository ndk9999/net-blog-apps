using Microsoft.AspNetCore.Mvc;
using TechBlog.Core.Contracts;
using TechBlog.Services.Blogs;
using TechBlog.Web.Models;

namespace TechBlog.Web.Components;

public class TagCloudWidget : ViewComponent
{
	private readonly IBlogRepository _blogRepository;

	public TagCloudWidget(IBlogRepository blogRepository)
	{
		_blogRepository = blogRepository;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var pagingParams = new GridRequest()
		{
			PageNumber = 1,
			PageSize = 20,
			SortColumn = "PostCount",
			SortOrder = "DESC"
		};
		var tagsList = await _blogRepository.GetPagedTagsAsync(pagingParams);

		return View(tagsList.ToList());
	}
}