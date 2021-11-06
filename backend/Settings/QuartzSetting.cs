using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace MyWeb.Settings
{
    public static class QuartzSetting
    {
        public static void AddQuartzService(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();
                options.UseSimpleTypeLoader();
                options.UseInMemoryStore();
            });

            services.AddQuartzHostedService(options =>
                options.WaitForJobsToComplete = true);
        }
    }
}
