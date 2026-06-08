namespace CulinaryBlog.Domain.Entities;

/// <summary>
/// Abstract base class cho tất cả Domain Entities.
/// Cung cấp audit fields chuẩn: Id, CreatedAt, UpdatedAt.
/// </summary>
/// <typeparam name="TId">Kiểu dữ liệu của khóa chính (Guid, int, ...)</typeparam>
public abstract class BaseEntity<TId>
{
	public TId Id { get; protected set; } = default!;

	// DateTimeOffset được sử dụng thay vì DateTime để đảm bảo rằng
	// thời gian được lưu trữ có thông tin về múi giờ, giúp tránh
	// các vấn đề liên quan đến múi giờ khi làm việc với dữ liệu
	// thời gian khi deploy ứng dụng ở nhiều múi giờ khác nhau.

	public DateTimeOffset CreatedAt { get; protected set; }

	public DateTimeOffset UpdatedAt { get; protected set; }

	// Protected constructor to prevent direct instantiation of the base class.
	// Chỉ Domain Entities kế thừa BaseEntity mới có thể gọi constructor này,
	// và tạo được instance.
	// EF Core sẽ sử dụng reflection để tạo instance của các Entity kế thừa,
	// nên parameterless constructor này cần phải có.
	protected BaseEntity()
	{
	}
}