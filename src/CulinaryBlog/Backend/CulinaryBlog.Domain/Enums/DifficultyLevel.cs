namespace CulinaryBlog.Domain.Enums;

/// <summary>
/// Mức độ khó của công thức nấu ăn.
/// Giá trị int được lưu vào DB dưới dạng string (xem RecipeConfiguration).
/// </summary>
public enum DifficultyLevel
{
	Easy = 1,       // Dễ - phù hợp cho người mới bắt đầu hoặc những công thức đơn giản.
	Medium = 2,     // Trung bình - phù hợp cho những người đã có kinh nghiệm nấu ăn, có thể yêu cầu một số kỹ thuật cơ bản.
	Hard = 3        // Khó - phù hợp cho những người có kinh nghiệm cao, có thể yêu cầu kỹ năng phức tạp hoặc nguyên liệu đặc biệt.
}