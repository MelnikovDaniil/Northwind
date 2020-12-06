using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Northwind.Infrastructure
{
    public static class RolesData
    {
        public static async Task SeedRoles(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var adminConfig = configuration.GetSection("UserAdmin");
                var adminRole = adminConfig["Role"];
                if (!await roleManager.RoleExistsAsync(adminRole))
                {
                    await roleManager.CreateAsync(new IdentityRole(adminRole));
                    var userName = adminConfig["UserName"];
                    var password = adminConfig["Password"];
                    var admin = new IdentityUser(userName);
                    await userManager.CreateAsync(admin, password);
                    var newUser = await userManager.FindByNameAsync(userName);
                    await userManager.SetLockoutEnabledAsync(newUser, false);
                    await userManager.AddToRoleAsync(newUser, adminRole);
                }
            }
        }
    }
}
