using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcClient.Models;

namespace MvcClient.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public UserViewModel UserViewModel { get; set; }
        public IActionResult UserInfo()
        {
            UserViewModel=new UserViewModel
            {
                Username = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value,
                UserId = User.Claims.FirstOrDefault(u=>u.Type=="sub")?.Value,
                Nickname = User.Claims.FirstOrDefault(u=>u.Type=="nickname")?.Value,
                PhoneNumber = User.Claims.FirstOrDefault(u=>u.Type== "phone_number")?.Value,
                CardNumber = User.Claims.FirstOrDefault(u=>u.Type=="cardnumber")?.Value,
                City = User.Claims.FirstOrDefault(u=>u.Type=="city")?.Value,
                Gender = User.Claims.FirstOrDefault(u=>u.Type==ClaimTypes.Gender)?.Value,
                Email = User.Claims.FirstOrDefault(u=>u.Type==ClaimTypes.Email)?.Value,
                University = User.Claims.FirstOrDefault(u=>u.Type=="university")?.Value,
                
            };
            return View(UserViewModel);
        }

        public IActionResult ForgotPassword()
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