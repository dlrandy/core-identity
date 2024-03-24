using System;
namespace IdentityManager.Models.ViewModels
{
	public class RolesViewModel
	{
		public ApplicationUser User { get; set; }
		public List<RolesSelection> RolesList { get; set; }
		public RolesViewModel()
		{
			RolesList = new List<RolesSelection>();
		}
	}

	public class RolesSelection {
		public string RoleName { get; set; }
		public bool IsSelected { get; set; }
	}
}

