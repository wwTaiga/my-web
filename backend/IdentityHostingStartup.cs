using Microsoft.AspNetCore.Hosting;
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
