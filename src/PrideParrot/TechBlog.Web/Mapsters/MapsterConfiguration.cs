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
			.Map(dest => dest.CategoryName, src => src.Category.Name);

		config.NewConfig<PostFilterModel, PostQuery>();
	}
}