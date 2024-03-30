using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace IdentityManager.Models
{
	public class ApplicationUser:IdentityUser
	{
		[Required]
		public string Name { get; set; }

        [NotMapped]
        public string RoleId { get; set; }

        [NotMapped]
        public string Role { get; set; }

        [NotMapped]
        public string UserClaim { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

