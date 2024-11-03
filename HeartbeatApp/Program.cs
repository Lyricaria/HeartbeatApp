using HeartbeatApp;
using HeartbeatApp.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

try
{
    Log.Information("Starting HeartbeatApp...");

    var builder = HostBuilderHelper.CreateHostBuilder(args);

    using var host = builder.Build();

    await host.StartAsync();

    var heartbeatEvent = host.Services.GetRequiredService<HeartbeatEvent>();
    await heartbeatEvent.StartAsync();

    Log.Information("HeartbeatApp is running. Press Ctrl+C to shut down.");

    await host.WaitForShutdownAsync();

    await heartbeatEvent.DisposeAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.CloseAndFlush();
}