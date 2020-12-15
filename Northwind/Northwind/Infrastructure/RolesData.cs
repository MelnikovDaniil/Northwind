using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Northwind.Infrastructure
{
    public static class RolesData
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var adminConfig = configuration.GetSection("UserAdmin");
                var userName = adminConfig["UserName"];
                if (userManager.FindByEmailAsync(userName).Result == null)
                {
                    var password = adminConfig["Password"];
                    var admin = new IdentityUser(userName);
                    await userManager.CreateAsync(admin, password);
                    var newUser = await userManager.FindByNameAsync(userName);
                    await userManager.SetLockoutEnabledAsync(newUser, false);
                    await userManager.AddToRoleAsync(newUser, "UserAdmin");
                }
            }
        }
    }
}
