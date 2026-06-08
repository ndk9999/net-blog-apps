using CulinaryBlog.Domain.Enums;
using CulinaryBlog.Domain.Extensions;
using NpgsqlTypes;
using System.Xml.Linq;

namespace CulinaryBlog.Domain.Entities;

/// <summary>
/// Công thức nấu ăn — entity trung tâm của Culinary Blog.
/// Liên kết với Category (N:1), Author/ApplicationUser (N:1),
/// Ingredients (1:N), và RecipeSteps (1:N).
/// </summary>
public class Recipe : BaseEntity<Guid>
{
	public string Title { get; private set; } = string.Empty;

	public string Slug { get; private set; } = string.Empty;

	public string? Description { get; private set; }

	public string? ThumbnailUrl { get; private set; }

	// Thời gian tính theo phút
	public int PrepTimeMinutes { get; private set; }

	public int CookTimeMinutes { get; private set; }

	public int Servings { get; private set; }

	public DifficultyLevel DifficultyLevel { get; private set; }

	public RecipeStatus Status { get; private set; } = RecipeStatus.Draft;

	// Khóa ngoại (Foreign keys)
	public Guid AuthorId { get; private set; }

	public Guid CategoryId { get; private set; }

	// SearchVector: cột tsvector cho PostgreSQL Full-Text Search.
	// Được cấu hình là "generated column" trong RecipeConfiguration (Lab 3).
	// Ở Lab 1 ta khai báo để EF Core biết, nhưng chưa dùng.
	public NpgsqlTsVector? SearchVector { get; private set; }

	// Navigation properties
	public Category? Category { get; private set; }

	public List<Ingredient> Ingredients { get; private set; } = [];

	public List<RecipeStep> Steps { get; private set; } = [];


	protected Recipe()
	{
	}

	public static Recipe Create(
		string title,
		string? description,
		int prepTimeMinutes,
		int cookTimeMinutes,
		int servings,
		DifficultyLevel difficultyLevel,
		Guid authorId,
		Guid categoryId,
		string? thumbnailUrl = null)
	{
		if (string.IsNullOrWhiteSpace(title))
			throw new ArgumentException("Recipe title cannot be null or empty.", nameof(title));

		var recipe = new Recipe
		{
			Id = Guid.NewGuid(),
			Title = title,
			Slug = title.Slugify(),
			Description = description,
			ThumbnailUrl = thumbnailUrl,
			PrepTimeMinutes = prepTimeMinutes,
			CookTimeMinutes = cookTimeMinutes,
			Servings = servings,
			DifficultyLevel = difficultyLevel,
			AuthorId = authorId,
			CategoryId = categoryId,
			CreatedAt = DateTimeOffset.UtcNow,
			UpdatedAt = DateTimeOffset.UtcNow
		};

		return recipe;
	}

	public void Update(
		string title,
		string? description,
		int prepTimeMinutes,
		int cookTimeMinutes,
		int servings,
		DifficultyLevel difficultyLevel,
		Guid categoryId,
		string? thumbnailUrl = null)
	{
		if (string.IsNullOrWhiteSpace(title))
			throw new ArgumentException("Recipe title cannot be null or empty.", nameof(title));

		Title = title;
		Slug = title.Slugify();
		Description = description;
		ThumbnailUrl = thumbnailUrl;
		PrepTimeMinutes = prepTimeMinutes;
		CookTimeMinutes = cookTimeMinutes;
		Servings = servings;
		DifficultyLevel = difficultyLevel;
		CategoryId = categoryId;
		UpdatedAt = DateTimeOffset.UtcNow;
	}

	public void Publish() => Status = RecipeStatus.Published;
	
	public void Archive() => Status = RecipeStatus.Archived;

	public void Unpublish() => Status = RecipeStatus.Draft;
}