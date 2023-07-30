﻿using System.ComponentModel.DataAnnotations;

namespace Task2.PL.ViewModels
{
	public class RegisterViewModel
	{
		public string FName { get; set; }
		public string LName { get; set; }

		[Required(ErrorMessage ="Email is required")]
		[EmailAddress(ErrorMessage ="Invalid Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]	
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is required")]
		[Compare("Password",ErrorMessage = "Confirm Password does not Password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }	

		public bool IsAgree { get; set; }
	}
}
