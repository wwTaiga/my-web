using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyWeb.Data;
using MyWeb.Models.Entities;

namespace MyWeb.Settings
{
    public static class AuthenticationSetting
    {
        public static void AddAuthentications(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["jwt"];
                        return Task.CompletedTask;
                    },

                    OnTokenValidated = context =>
                    {
                        /* var userMachine = context.HttpContext.RequestServices */
                        /*         .GetRequiredService<UserManager<LoginUser>>(); */
                        /* var user = userMachine.GetUserAsync(context.HttpContext.User).Result; */
                        /*
                        /* if (user is null) */
                        /* { */
                        /*     context.Fail("UnAuthorized"); */
                        /* } */

                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            })
            .AddIdentityCookies(o => { });

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            });

            services.AddIdentityCore<LoginUser>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddSignInManager()
                .AddEntityFrameworkStores<DataContext>();
        }
    }
}
