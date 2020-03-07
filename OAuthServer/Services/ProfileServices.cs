using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using OAuthServer.Models;

namespace OAuthServer.Services
{
    public class ProfileServices : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileServices(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value;
            var user = await _userManager.FindByIdAsync(subjectId);
            if (user==null)
            {
                throw new ArgumentException("Invalid subject identifier");
            }

            var claims = GetClaimsFromUser(user);
            context.IssuedClaims = claims.ToList();
        }

        private IEnumerable<Claim> GetClaimsFromUser(ApplicationUser user)
        {
            var claims=new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject,user.Id),
                new Claim(JwtClaimTypes.PreferredUserName,user.UserName),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName)
            };
            if (!string.IsNullOrWhiteSpace(user.NickName))
            {
                claims.Add(new Claim("nickname",user.NickName));
            }

            if (!string.IsNullOrWhiteSpace(user.CardNumber))
            {
                claims.Add(new Claim("cardnumber",user.CardNumber));
            }

            if (!string.IsNullOrWhiteSpace(user.University))
            {
                claims.Add(new Claim("university", user.University));
            }

            if (!string.IsNullOrWhiteSpace(user.Gender))
            {
                claims.Add(new Claim(JwtClaimTypes.Gender,user.Gender));
            }
            if (!string.IsNullOrWhiteSpace(user.City))
            {
                claims.Add(new Claim("city", user.City));
            }
            if (_userManager.SupportsUserPhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                claims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                    new Claim(JwtClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
                });
            }

            return claims;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
