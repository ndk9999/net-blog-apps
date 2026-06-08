using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TatBlog.WebApp.Areas.Admin.Models;

public class PostEditModel
{
	public int Id { get; set; }

	[DisplayName("Tiêu đề")]
	[Required(ErrorMessage = "Tiêu đề không được để trống")]
	[MaxLength(500, ErrorMessage = "Tiêu đề tối đa 500 ký tự")]
	public string Title { get; set; }

	[DisplayName("Giới thiệu")]
	[Required]
	public string ShortDescription { get; set; }

	[DisplayName("Nội dung")]
	[Required]
	public string Description { get; set; }

	[DisplayName("Metadata")]
	[Required]
	public string Meta { get; set; }

	[DisplayName("Chọn hình ảnh")]
	public IFormFile ImageFile { get; set; }

	[DisplayName("Hình hiện tại")]
	public string ImageUrl { get; set; }
	
	[DisplayName("Xuất bản ngay")]
	public bool Published { get; set; }

	[DisplayName("Chủ đề")]
	[Required]
	public int CategoryId { get; set; }

	[DisplayName("Tác giả")]
	[Required]
	public int AuthorId { get; set; }

	[DisplayName("Từ khóa (mỗi từ 1 dòng)")]
	[Required]
	public string SelectedTags { get; set; }


	public IEnumerable<SelectListItem> AuthorList { get; set; }

	public IEnumerable<SelectListItem> CategoryList { get; set; }


	public List<string> GetSelectedTags()
	{
		return (SelectedTags ?? "")
			.Split(new[] {',', ';', '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
			.ToList();
	}
}