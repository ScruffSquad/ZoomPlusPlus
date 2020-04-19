using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ScruffCalendar.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await HttpContext.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}