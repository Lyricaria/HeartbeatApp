using HeartbeatApp.Jobs;
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
            var natsUrl = configuration.GetSection("Nats")["Url"];

            services.AddSingleton(new HeartbeatEvent(natsUrl));

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
        }
    }
}
