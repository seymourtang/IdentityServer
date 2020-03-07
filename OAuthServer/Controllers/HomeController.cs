using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
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
                var user = new ApplicationUser
                {
                    UserName = viewModel.Username,
                    Email = viewModel.Username,
                    Gender = "Male",
                    University = "Tongji University",
                    City = "Shanghai",
                    NickName = "三梦哥",
                    CardNumber = "1941756"
                };
                var registerResult = await _userManager.CreateAsync(user, viewModel.Password);
                //await _userManager.AddToRoleAsync(user,"admin");
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

        [HttpGet]
        public async Task<IActionResult> AddClaims()
        {
            var user =await _userManager.GetUserAsync(HttpContext.User);
            //await _userManager.AddClaimsAsync(user,new List<Claim>
            //{
            //   new Claim("gender","女"),
            //   new Claim("role","admin")
            //});
             User.HasClaim(m=>m.Value=="role");
             await Task.CompletedTask;
             var claims = User.Claims.Select(claim => new { claim.Type, claim.Value }).ToArray();
            return Json(claims);
        }

        [Route("oauth2/logout")]
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            if (User.Identity.IsAuthenticated==false)
            {
                return await Logout(new LogoutViewModel
                {
                    LogoutId = logoutId
                });
            }

            var context = await _interactionService.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt==false)
            {
                return await Logout(new LogoutViewModel
                {
                    LogoutId = logoutId
                });
            }
            var vm=new LogoutViewModel
            {
                LogoutId = logoutId
            };
            return View(vm);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            var idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
            if (idp!=null&&idp!=IdentityServerConstants.LocalIdentityProvider)
            {
                if (model.LogoutId==null)
                {
                    model.LogoutId = await _interactionService.CreateLogoutContextAsync();
                }

                string url = "/Home/Logout?logoutId=" + model.LogoutId;
                try
                {
                    await HttpContext.SignOutAsync(idp, new AuthenticationProperties
                    {
                        RedirectUri = url
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "LOGOUT ERROR: {ExceptionMessage}", e.Message);
                }
            }

            await HttpContext.SignOutAsync();
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            HttpContext.User=new ClaimsPrincipal(new ClaimsIdentity());
            var logout = await _interactionService.GetLogoutContextAsync(model.LogoutId);
            return Redirect(logout?.PostLogoutRedirectUri);
        }
    }
}
