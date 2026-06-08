namespace CulinaryBlog.Domain.Entities;

public class RecipeStep : BaseEntity<Guid>
{
	public Guid RecipeId { get; private set; }

	// Step number trong công thức, bắt đầu từ 1. Ví dụ: 1, 2, 3...
	public int StepNumber { get; private set; }

	// Tiêu đề mô tả ngắn gọn cho bước này, có thể null nếu không cần thiết.
	// Ví dụ: "Chuẩn bị nguyên liệu", "Nấu nước dùng", "Trình bày món ăn".
	public string? Title { get; private set; }

	public string Description { get; private set; } = string.Empty;

	// URL hình ảnh minh họa cho bước này, có thể null nếu không có hình ảnh.
	public string? ImageUrl { get; private set; }

	// Thời gian ước tính để hoàn thành bước này, tính theo phút. Có thể null nếu không xác định được.
	public int? DurationInMinutes { get; private set; }

	public Recipe? Recipe { get; private set; }


	protected RecipeStep()
	{
	}

	public static RecipeStep Create(
		Guid recipeId,
		int stepNumber,
		string description,
		string? title = null,
		string? imageUrl = null,
		int? durationInMinutes = null)
	{
		if (string.IsNullOrWhiteSpace(description))
			throw new ArgumentException("Description cannot be null or empty.", nameof(description));

		if (stepNumber < 1)
			throw new ArgumentException("StepNumber must be greater than 0.", nameof(stepNumber));
		
		var step = new RecipeStep
		{
			Id = Guid.NewGuid(),
			RecipeId = recipeId,
			StepNumber = stepNumber,
			Title = title?.Trim(),
			Description = description.Trim(),
			ImageUrl = imageUrl?.Trim(),
			DurationInMinutes = durationInMinutes,
			CreatedAt = DateTimeOffset.UtcNow,
			UpdatedAt = DateTimeOffset.UtcNow
		};

		return step;
	}

	public void Update(
		int stepNumber,
		string description,
		string? title = null,
		string? imageUrl = null,
		int? durationInMinutes = null)
	{
		if (string.IsNullOrWhiteSpace(description))
			throw new ArgumentException("Description cannot be null or empty.", nameof(description));

		if (stepNumber < 1)
			throw new ArgumentException("StepNumber must be greater than 0.", nameof(stepNumber));

		StepNumber = stepNumber;
		Title = title?.Trim();
		Description = description.Trim();
		ImageUrl = imageUrl?.Trim();
		DurationInMinutes = durationInMinutes;
		UpdatedAt = DateTimeOffset.UtcNow;
	}
}