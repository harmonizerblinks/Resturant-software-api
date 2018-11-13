using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Resturant.Models;
using System.Linq;
using Resturant.Services;
using Resturant.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Resturant.Services;

namespace Resturant.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IEmailSender _emailSender;
        public IConfiguration Configuration { get; }

        public ConfirmEmailModel(UserManager<AppUser> userManager, AppDbContext context, IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _emailSender = emailSender;
            Configuration = configuration;
        }

        public string Message { get; set; }
        public object AppSettings { get; private set; }

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
                else
                {
                    Message = $"User with email : {user?.UserName} confirmed successfully";
                    var use = _context.Users.Where(u => u.UserName == user.UserName).Include(c=>c.Employee).FirstOrDefault();

                    var Url = "";
                    await _emailSender.SendEmailConfirmAsync(Url, use.Email, use.Employee.FullName);

                    Response.Redirect(location: Url);
                }
            }

            return null;
        }

    }
}