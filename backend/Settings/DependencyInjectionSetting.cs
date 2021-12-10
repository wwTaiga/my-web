using Microsoft.Extensions.DependencyInjection;
using MyWeb.Data;
using MyWeb.Repositories;
using MyWeb.Services;

namespace MyWeb.Settings
{
    public static class DependencyInjectionSetting
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IDataContext>(provider =>
                provider.GetService<DataContext>());

            services.AddScoped<ILoginUserRepo, LoginUserRepo>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRepoService, RepoService>();
        }
    }
}
