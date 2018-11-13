using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Resturant.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Resturant.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmPasswordResetModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public ConfirmPasswordResetModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public string Message { get; set; }

        public async Task<IActionResult> OnGet(string userId, string code)
        {
            if (userId == null || code == null)
            {
                Message = "User Id and code not supplied";
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                Message = ($"Unable to load user with ID '{userId}'.");
            }
            else
            {
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (!result.Succeeded)
                {
                    Message = ($"Error confirming email for user with ID '{userId}':");
                }
            }

            Message = $"User with email : {user?.UserName} confirmed successfully";
            Response.Redirect(location: "http://kenrude.acyst.tech/Login");
            return null;

    }
    }
}