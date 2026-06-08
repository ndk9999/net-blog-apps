namespace CulinaryBlog.Domain.Entities;

/// <summary>
/// Nguyên liệu của một công thức cụ thể.
/// VD: { Name="Thịt bò", Quantity="300", Unit="gram", OrderIndex=1 }
/// </summary>
public class Ingredient : BaseEntity<Guid>
{
	public Guid RecipeId { get; private set; }

	public string Name { get; private set; } = string.Empty;

	public string Quantity { get; private set; } = string.Empty;

	public string? Unit { get; private set; }

	// Thứ tự hiển thị nguyên liệu trong công thức, bắt đầu từ 1. Ví dụ: 1, 2, 3...
	public int OrderIndex { get; private set; }

	// Navigation property: Một Ingredient thuộc về một Recipe.
	public Recipe? Recipe { get; private set; }

	protected Ingredient()
	{
	}

	public static Ingredient Create(Guid recipeId, string name, string quantity, string? unit, int orderIndex)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Ingredient name cannot be null or empty.", nameof(name));

		if (string.IsNullOrWhiteSpace(quantity))
			throw new ArgumentException("Ingredient quantity cannot be null or empty.", nameof(quantity));

		if (orderIndex < 1)
			throw new ArgumentException("OrderIndex must be greater than 0.", nameof(orderIndex));

		var ingredient = new Ingredient
		{
			Id = Guid.NewGuid(),
			RecipeId = recipeId,
			Name = name.Trim(),
			Quantity = quantity.Trim(),
			Unit = unit?.Trim(),
			OrderIndex = orderIndex,
			CreatedAt = DateTimeOffset.UtcNow,
			UpdatedAt = DateTimeOffset.UtcNow
		};

		return ingredient;
	}

	public void Update(string name, string quantity, string? unit, int orderIndex)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Ingredient name cannot be null or empty.", nameof(name));

		if (string.IsNullOrWhiteSpace(quantity))
			throw new ArgumentException("Ingredient quantity cannot be null or empty.", nameof(quantity));

		if (orderIndex < 1)
			throw new ArgumentException("OrderIndex must be greater than 0.", nameof(orderIndex));

		Name = name.Trim();
		Quantity = quantity.Trim();
		Unit = unit?.Trim();
		OrderIndex = orderIndex;
		UpdatedAt = DateTimeOffset.UtcNow;
	}
}