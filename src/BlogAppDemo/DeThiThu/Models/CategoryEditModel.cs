using System.ComponentModel.DataAnnotations;

namespace DeThiThu.Models;

public class CategoryEditModel
{
	public int Id { get; set; }
	
	public string Name { get; set; }

	public bool ShowOnMenu { get; set; }
}