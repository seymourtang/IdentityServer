using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyApp.Controllers
{
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private string ClientId = "mobile";
        private string ClientSecret = "DC5E30EC-B2AF-4FC2-9DD9-6D80382A63B8";
        private string Scope = "openid%20profile%20api1";
        private string RedirectUri = "https%3A%2F%2Flocalhost%3A44303%2Fsignin-oidc";
        private string State = "ajhdbajksghdiutgertergdfgdasd";
        private string UserInfoUri = "https://localhost:44300/connect/userinfo";
        [Route("/login")]
        public  IActionResult Login()
        {
            var url = $"https://localhost:44300/connect/authorize?response_type=code&state={State}&client_id={ClientId}&scope={Scope}&redirect_uri={RedirectUri}";
            return Redirect(url);
        }

        [Route("/signin-oidc")]
        public async Task<IActionResult> Index(string code)
        {
            var parms=new Dictionary<string,string>();
            parms.Add("client_id",ClientId);
            parms.Add("client_secret",ClientSecret);
            parms.Add("code",code);
            parms.Add("grant_type", "authorization_code");
            parms.Add("redirect_uri", "https://localhost:44303/signin-oidc");
            var httpclient=new HttpClient();
            var reponse = await httpclient.PostAsync("https://localhost:44300/connect/token",
                new FormUrlEncodedContent(parms));
            if (!reponse.IsSuccessStatusCode)
            {
                return Ok(new
                {
                    message = "Failed"
                });
            }
            var data = await reponse.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            var access_token = responseData.GetValueOrDefault("access_token");
            httpclient.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Bearer",access_token);
            var userInfoResponse = await httpclient.GetAsync(UserInfoUri);
            if (!userInfoResponse.IsSuccessStatusCode)
            {
                return Ok(new
                {
                    message = "Ok",
                    data = responseData
                });
            }

            var userInfoData = JsonConvert.DeserializeObject<Dictionary<string, String>>(await  userInfoResponse.Content.ReadAsStringAsync());
            return Ok(new
            {
                message = "Ok",
                data = responseData,
                userInfo=userInfoData
            });

        }

    }
}