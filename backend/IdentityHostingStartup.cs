using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyWeb.Data;
using MyWeb.Models;
using MyWeb.Settings;

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
                });

                services.AddDbContext<DataContext>(options =>
                {
                    var settings = context.Configuration.GetSection(nameof(PostgresDbSettings))
                        .Get<PostgresDbSettings>();
                    options.UseNpgsql(settings.ConnectionString);
                });

                services.AddDefaultIdentity<LoginUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<DataContext>();
            });
        }
    }
}
