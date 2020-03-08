using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;

namespace OAuthServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
            };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource("api1"),
                new ApiResource("api2")
            };

        public static IEnumerable<Client> GetClients()=>
            new List<Client>
            {
                new Client
                {
                    ClientId = "mobile",
                    ClientSecrets =
                    {
                        new Secret("DC5E30EC-B2AF-4FC2-9DD9-6D80382A63B8".ToSha256())
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris =
                    {
                        "https://localhost:44303/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:44303/sigin-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        "api1",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    AllowOfflineAccess = true,
                    RequireConsent = false
                },
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets =
                    {
                        new Secret("8A1F6429-8E2D-478B-92FD-AE55E8BBEE08".ToSha256())
                    },
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RedirectUris =
                    {
                        "https://localhost:44302/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:44302/signout-callback-oidc"
                        
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        "api1",
                    },
                    AllowOfflineAccess = true,
                    RequireConsent = false
                }

            };
    }
}
