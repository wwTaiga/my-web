using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyWeb.Core.Models.Entities;
using MyWeb.Core.Models.Enums;
using MyWeb.Core.Models.Settings;

namespace MyWeb.Core.Data.ExternalMasterData;

public static class IdentityData
{
    public static async Task InitializeData(IServiceProvider serviceProvider,
        IConfiguration configuration)
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

            var adminAccountSettings = configuration.GetSection(nameof(AdminAccountSettings))
                .Get<AdminAccountSettings>();
            UserManager<LoginUser> userManager =
                serviceScope.ServiceProvider.GetService<UserManager<LoginUser>>();
            if (!dataContext.LoginUser.Any(u => u.UserName == adminAccountSettings.Username))
            {
                LoginUser admin = new()
                {
                    UserName = adminAccountSettings.Username,
                    Email = adminAccountSettings.Email,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, adminAccountSettings.Password);
            }


            LoginUser user = await userManager.FindByNameAsync(adminAccountSettings.Username);
            if (!await userManager.IsInRoleAsync(user, Role.SuperAdmin.ToString()))
            {
                await userManager.AddToRoleAsync(user, Role.SuperAdmin.ToString());
            }
        }
    }
}
