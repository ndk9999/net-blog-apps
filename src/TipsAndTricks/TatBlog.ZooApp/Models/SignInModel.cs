using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TatBlog.ZooApp.Models;

	public class SignInModel
	{
		[BindProperty(Name = "name")]
		[Required(ErrorMessage = "Please enter username")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[MinLength(6, ErrorMessage = "Password is too short")]
		public string Password { get; set; }
	}
