using HeartbeatApp.Interfaces;
using HeartbeatApp.Jobs;
using HeartbeatApp.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace HeartbeatApp.Helpers
{
    public static class ServiceConfigurator
    {
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = hostContext.Configuration;
            services.Configure<NatsOptions>(configuration.GetSection("Nats"));

            services.AddSingleton<IHeartbeatEvent, HeartbeatEvent>();

            //Quartz
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = new JobKey("HeartbeatJob");

                q.AddJob<HeartbeatJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("HeartbeatJob-trigger")
                    .WithCronSchedule("0/15 * * * * ?")); // Every 15 seconds
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            services.AddHostedService<HeartbeatHostedService>();
        }
    }
}
