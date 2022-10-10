using Mapster;
using TechBlog.Core.DTO;
using TechBlog.Core.Entities;
using TechBlog.Web.Models;

namespace TechBlog.Web.Mapsters;

public class MapsterConfiguration : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Post, PostItem>()
			.Map(dest => dest.CategoryName, src => src.Category.Name)
			.Map(dest => dest.Tags, src => src.Tags.Select(x => x.Name));

		config.NewConfig<PostFilterModel, PostQuery>()
			.Map(dest => dest.PublishedOnly, src => false);

		config.NewConfig<PostEditModel, Post>()
			.Ignore(dest => dest.Id);

		config.NewConfig<Post, PostEditModel>()
			.Map(dest => dest.SelectedTags, src => string.Join("\r\n", src.Tags.Select(x => x.Name)))
			.Ignore(dest => dest.CategoryList)
			.Ignore(dest => dest.TagList);

		config.NewConfig<CategoryEditModel, Category>()
			.Ignore(dest => dest.Id);
	}
}