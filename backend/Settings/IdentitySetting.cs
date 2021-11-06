using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyWeb.Data;
using MyWeb.Models.Entities;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace MyWeb.Settings
{
    public static class IdentitySetting
    {
        public static void AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<LoginUser, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
            });
        }
    }
}
