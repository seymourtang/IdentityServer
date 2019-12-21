using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OAuthServer.Models;

namespace OAuthServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IIdentityServerInteractionService interactionService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _interactionService = interactionService;
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Route("oauth2/authorize")]
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl="")
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index");
            }
            var externalProviders = await _signInManager.GetExternalAuthenticationSchemesAsync();
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalProviders = externalProviders
            });
        }
        [Route("oauth2/authorize")]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(viewModel.Username);
                if (user != null)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(viewModel.Username, viewModel.Password, false, false);
                    if (signInResult.Succeeded)
                    {
                        return Redirect(viewModel.ReturnUrl);
                    }
                }

            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = viewModel.Username,
                    Email = ""
                };
                var registerResult = await _userManager.CreateAsync(user, viewModel.Password);
                if (registerResult.Succeeded)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(user, viewModel.Password, false, false);
                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return RedirectToAction("Index");

        }

        [Route("oauth2/logout")]
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
