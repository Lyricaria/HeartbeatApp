// NATS URL configuration
using HeartbeatApp;
using HeartbeatApp.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

var natsUrl = "nats://localhost:4222";

// Create the HostBuilder
var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // Register HeartbeatEvent as a singleton service
        services.AddSingleton(new HeartbeatEvent(natsUrl));

        // Add Quartz services
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            // Register the job and trigger
            var jobKey = new JobKey("HeartbeatJob");

            q.AddJob<HeartbeatJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("HeartbeatJob-trigger")
                .WithCronSchedule("0/15 * * * * ?")); // Every 15 seconds
        });

        // Add Quartz.NET hosted service
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    });

// Build and start the host
using var host = builder.Build();
await host.StartAsync();

// Start the HeartbeatEvent connection
var heartbeatEvent = host.Services.GetRequiredService<HeartbeatEvent>();
await heartbeatEvent.StartAsync();

Console.WriteLine("HeartbeatApp is running. Press Ctrl+C to shut down.");

// Wait for the application to exit
await host.WaitForShutdownAsync();

// Dispose of the HeartbeatEvent when the application is shutting down
await heartbeatEvent.DisposeAsync();