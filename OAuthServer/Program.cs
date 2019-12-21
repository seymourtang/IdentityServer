using System.Linq;
using System.Security.Claims;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OAuthServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //using (var scope = host.Services.CreateScope())
            //{
            //    var userManager = scope.ServiceProvider
            //        .GetRequiredService<UserManager<IdentityUser>>();

            //    var user = new IdentityUser("bob");
            //    userManager.CreateAsync(user, "password").GetAwaiter().GetResult();

            //    scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>()
            //        .Database.Migrate();

            //    var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            //    context.Database.Migrate();

            //    if (!context.Clients.Any())
            //    {
            //        foreach (var client in Configuration.GetClients())
            //        {
            //            context.Clients.Add(client.ToEntity());
            //        }
            //        context.SaveChanges();
            //    }

            //    if (!context.IdentityResources.Any())
            //    {
            //        foreach (var resource in Configuration.GetIdentityRessources())
            //        {
            //            context.IdentityResources.Add(resource.ToEntity());
            //        }
            //        context.SaveChanges();
            //    }

            //    if (!context.ApiResources.Any())
            //    {
            //        foreach (var resource in Configuration.GetApiResources())
            //        {
            //            context.ApiResources.Add(resource.ToEntity());
            //        }
            //        context.SaveChanges();
            //    }
            //}
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
