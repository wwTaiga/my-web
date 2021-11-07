using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyWeb.Data;
using OpenIddict.Quartz;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace MyWeb.Settings
{
    public static class OpenIddictSetting
    {
        public static void AddOpenIddictService(this IServiceCollection services)
        {
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    // Configure OpenIddict to use the Entity Framework Core stores and models.
                    // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
                    options.UseEntityFrameworkCore()
                        .UseDbContext<DataContext>();

                    // Configure OpenIddict to use Quartz to clean invalid and expired token
                    options.UseQuartz();
                })
                .AddServer(options =>
                {
                    options.SetTokenEndpointUris("/connect/token");

                    // Authentication Flow
                    // More info:
                    // https://documentation.openiddict.com/guide/choosing-the-right-flow.html
                    options.AllowPasswordFlow()
                        .AllowRefreshTokenFlow();

                    // Set token default lifetime
                    options.SetAccessTokenLifetime(TimeSpan.FromMinutes(5))
                        .SetRefreshTokenLifetime(TimeSpan.FromHours(1));

                    // Add scope/permission, to allow client request the info
                    options.RegisterScopes(
                        Scopes.Profile
                    );

                    // Set the timespan for reuse refresh still can be reuse after redeem
                    options.SetRefreshTokenReuseLeeway(TimeSpan.FromSeconds(5));

                    // Allow request without authenticated client id and client name
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

            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
            });

            // Configure OpenIddict Quartz job to clean:
            // Token created more than MinimumTokenLifespan, default 14 days
            // Authorization created more than MinimumAuthorizationLifeSpan, default 14 days
            services.Configure<OpenIddictQuartzOptions>(options =>
            {
                options.MinimumTokenLifespan = TimeSpan.FromHours(1);
            });

            // Use OpenIddict as default authentication scheme
            // Reference:
            // https://github.com/openiddict/openiddict-core/issues/1182
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });
        }
    }
}
