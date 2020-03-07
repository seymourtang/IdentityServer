using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MvcClient.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public IActionResult UserInfo()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            return SignOut("Cookie", "oidc");
        }
    }
}