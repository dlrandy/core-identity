﻿using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityManager.Models.ViewModels
{
	public class RegisterViewModel
	{
		[Required]
		public string Name { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
        public string Password { get; set; }
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password",ErrorMessage = "The pasword and confirmation password donot match.")]
		public string ConfirmPassword { get; set; }
	}
}

