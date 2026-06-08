namespace DeThiThu.Entities;

public class Subscriber
{
	// Mã số người đăng ký
	public int Id { get; set; }

	// Địa chỉ email của người đăng ký
	public string Email { get; set; }

	// Ngày giờ đăng ký
	public DateTime SubscribedDate { get; set; }

}