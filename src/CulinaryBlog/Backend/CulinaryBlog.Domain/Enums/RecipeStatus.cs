namespace CulinaryBlog.Domain.Enums;

/// <summary>
/// Trạng thái xuất bản của công thức.
/// Workflow: Draft → Published → Archived.
/// </summary>
public enum RecipeStatus
{
	Draft = 0,      // Bản nháp - công thức đang được soạn thảo, chưa hoàn chỉnh và chưa được xuất bản.
	Published = 1,  // Đã xuất bản - công thức đã được duyệt và hiển thị công khai cho người dùng.
	Archived = 2    // Đã lưu trữ - công thức đã được ẩn khỏi listing nhưng URL vẫn có thể truy cập.
}