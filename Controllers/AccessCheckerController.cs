using IdentityManager.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers
{
    [Authorize]
    public class AccessCheckerController : Controller
    {
        //Anyone can access this
        [AllowAnonymous]
        public IActionResult AllAccess()
        {
            return View();
        }


        //Anyone that has logged in can access
        public IActionResult AuthorizedAccess()
        {
            return View();
        }


        //account with role of user can access
        [Authorize(Roles = $"{SD.Admin},{SD.User}")]
        public IActionResult UserOrAdminRoleAccess()
        {
            return View();
        }


        [Authorize(Policy = SD.AdminAndUser)]
        public IActionResult UserAndAdminRoleAccess()
        {
            return View();
        }


        //account with role of admin can access
        //[Authorize(Roles = SD.Admin)]
        [Authorize(Policy = SD.Admin)]
        public IActionResult AdminRoleAccess()
        {
            return View();
        }

        //account with admin role and create Claim can access
        [Authorize(Policy = SD.AdminRole_CreateClaim)]
        public IActionResult Admin_CreateAccess()
        {
            return View();
        }

        //account with admin role and (create & Edit & Delete) Claim can access (AND NOT OR)
        [Authorize(Policy = SD.AdminRole_CreateEditDeleteClaim)]
        public IActionResult Admin_Create_Edit_DeleteAccess()
        {
            return View();
        }


        [Authorize(Policy = SD.AdminRole_CreateEditDeleteClaim_OR_SuperAdminRole)]
        public IActionResult Admin_Create_Edit_DeleteAccess_OR_SuperAdminRole()
        {
            return View();
        }

        [Authorize(Policy = SD.AdminWithMoreThan1000Days)]
        public IActionResult OnlyBhrugen()
        {
            return View();
        }

        [Authorize(Policy = SD.FirstNameAuth)]
        public IActionResult FirstNameAuth()
        {
            return View();
        }
    }
}