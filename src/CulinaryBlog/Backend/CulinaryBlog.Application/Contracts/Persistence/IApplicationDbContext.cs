using CulinaryBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CulinaryBlog.Application.Contracts.Persistence;

public interface IApplicationDbContext
{
	DbSet<Category> Categories { get; }

	DbSet<Recipe> Recipes { get; }
	
	DbSet<Ingredient> Ingredients { get; }
	
	DbSet<RecipeStep> RecipeSteps { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}