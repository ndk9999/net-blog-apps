namespace DeThiThu.Entities;

public class Candy
{
	// Mã số
	public int Id { get; set; }

	// Tên mặt hàng kẹo
	public string Name { get; set; }

	// Giá bán (đơn vị là USD, luôn lớn hơn 0 và
	// có phần lẻ thập phân. Ví dụ: 5.99).
	public decimal Price { get; set; }
	
	// Ngày hết hạn sử dụng
	public DateTime ExpirationDate { get; set; }

	// Mã số danh mục (Mỗi mặt hàng kẹo phải thuộc
	// đúng một danh mục)
	public int CategoryId { get; set; }


	public Category Category { get; set; }
}