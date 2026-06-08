namespace CulinaryBlog.Application.DTOs;

public record RecipeSummaryDto(
    Guid Id,
    string Title,
    string Slug,
    string? ThumbnailUrl,
    int PrepTimeMinutes,
    int CookTimeMinutes,
    string DifficultyLevel,
    string Status
);