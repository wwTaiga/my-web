using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyWeb.Data;
using MyWeb.Models.Entities;

namespace MyWeb.Settings
{
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
}
