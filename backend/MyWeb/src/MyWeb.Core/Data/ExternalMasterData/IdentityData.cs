using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyWeb.Core.Models.Entities;
using MyWeb.Core.Models.Enums;

namespace MyWeb.Core.Data.ExternalMasterData;

public static class IdentityData
{
    public static async Task InitializeData(IServiceProvider serviceProvider)
    {
        using (var serviceScope =
            serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var dataContext = serviceScope.ServiceProvider.GetService<DataContext>();
            var roles = Enum.GetNames(typeof(Role));

            foreach (string role in roles)
            {
                var roleManager =
                    serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                if (!dataContext.Roles.Any(r => r.Name == role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            UserManager<LoginUser> userManager =
                serviceScope.ServiceProvider.GetService<UserManager<LoginUser>>();
            if (!dataContext.LoginUser.Any(u => u.UserName == "Admin"))
            {
                LoginUser admin = new()
                {
                    UserName = "Admin",
                    Email = "admin@admin.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "Admin@1234");
            }


            LoginUser user = await userManager.FindByNameAsync("Admin");
            if (!await userManager.IsInRoleAsync(user, Role.SuperAdmin.ToString()))
            {
                await userManager.AddToRoleAsync(user, Role.SuperAdmin.ToString());
            }
        }
    }
}
