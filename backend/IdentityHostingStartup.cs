using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyWeb.Data;
using MyWeb.Models;

[assembly: HostingStartup(typeof(MyWeb.IdentityHostingStartup))]
namespace MyWeb
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.Configure<IdentityOptions>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = false;
                });

                services.AddDefaultIdentity<LoginUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<DataContext>();
            });
        }
    }
}
