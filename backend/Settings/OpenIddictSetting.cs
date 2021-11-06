
using System;
using Microsoft.Extensions.DependencyInjection;
using MyWeb.Data;

namespace MyWeb.Settings
{
    public static class OpenIddictSetting
    {
        public static void AddOpenIddictService(this IServiceCollection services)
        {
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<DataContext>();

                    options.UseQuartz();
                })
                .AddServer(options =>
                {
                    options.SetTokenEndpointUris("/connect/token");

                    options.AllowPasswordFlow()
                        .AllowRefreshTokenFlow();

                    options.SetAccessTokenLifetime(TimeSpan.FromMinutes(5))
                        .SetRefreshTokenLifetime(TimeSpan.FromHours(1));

                    options.AcceptAnonymousClients();

                    options.AddDevelopmentSigningCertificate()
                        .AddDevelopmentEncryptionCertificate();

                    options.UseAspNetCore()
                        .EnableTokenEndpointPassthrough()
                        .DisableTransportSecurityRequirement();
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });
        }
    }
}
