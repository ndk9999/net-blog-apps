using DeThiThu.Contexts;
using DeThiThu.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeThiThu.Services;

public class CandyService : ICandyService
{
	private readonly CandyContext _context;

	public CandyService(CandyContext context)
	{
		_context = context;
	}

	public async Task<IList<Candy>> GetCandiesAsync(
		string name = null,
		int? categoryId = null,
		string categoryName = null,
		decimal? minPrice = null,
		decimal? maxPrice = null,
		CancellationToken cancellationToken = default)
	{
		var today = DateTime.Now.Date;
		var candyQuery = _context.Candies.Where(x => x.ExpirationDate > today);

		if (!string.IsNullOrEmpty(name))
		{
			candyQuery = candyQuery.Where(x => x.Name.Contains(name));
		}

		if (!string.IsNullOrEmpty(categoryName))
		{
			candyQuery = candyQuery.Where(x => x.Category.Name == categoryName);
		}

		if (minPrice >= 0 && maxPrice >= 0 && minPrice <= maxPrice)
		{
			candyQuery = candyQuery.Where(x => x.Price >= minPrice && x.Price <= maxPrice);
		}

		if (categoryId > 0)
		{
			candyQuery = candyQuery.Where(x => x.CategoryId == categoryId);
		}

		return await candyQuery
			.OrderBy(x => x.Name)
			.ToListAsync(cancellationToken);
	}

	public async Task<Category> AddOrUpdateCategoryAsync(
		string categoryName,
		bool showOnMenu = false,
		int? categoryId = null,
		CancellationToken cancellationToken = default)
	{
		var category = categoryId > 0
			? _context.Categories.Find(categoryId.Value)
			: null;

		if (category == null)
		{
			category = new Category()
			{
				Name = categoryName,
				ShowOnMenu = showOnMenu
			};
			_context.Categories.Add(category);
		}
		else
		{
			category.Name = categoryName;
			category.ShowOnMenu = showOnMenu;
		}

		await _context.SaveChangesAsync(cancellationToken);
		return category;
	}
}