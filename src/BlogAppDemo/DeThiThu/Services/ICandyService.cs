using DeThiThu.Entities;

namespace DeThiThu.Services;

public interface ICandyService
{
	Task<IList<Candy>> GetCandiesAsync(
		string name = null, 
		int? categoryId = null, 
		string categoryName = null,
		decimal? minPrice = null, 
		decimal? maxPrice = null,
		CancellationToken cancellationToken = default);

	Task<Category> AddOrUpdateCategoryAsync(
		string categoryName,
		bool showOnMenu = false,
		int? categoryId = null,
		CancellationToken cancellationToken = default);
}