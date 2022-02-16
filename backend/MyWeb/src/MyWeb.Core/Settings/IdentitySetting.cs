using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyWeb.Core.Data;
using MyWeb.Core.Models.Entities;

namespace MyWeb.Core.Settings;

public static class IdentitySetting
{
    public static void AddIdentityService(this IServiceCollection services)
    {
        services.AddIdentity<LoginUser, IdentityRole>(o =>
        {
            o.User.RequireUniqueEmail = true;
            o.SignIn.RequireConfirmedEmail = false;
        })
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager();

    }
}
