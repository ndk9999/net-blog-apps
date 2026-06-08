namespace DeThiThu.Entities;

public class Category
{
	// Mã số danh mục
	public int Id { get; set; }

	// Tên danh mục
	public string Name { get; set; }

	// Cho biết hiển thị lên menu hay không
	public bool ShowOnMenu { get; set; }
}