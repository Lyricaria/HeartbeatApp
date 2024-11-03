using HeartbeatApp.Helpers;
using Microsoft.Extensions.Hosting;
using Serilog;

try
{
    Log.Information("Starting HeartbeatApp...");

    var builder = HostBuilderHelper.CreateHostBuilder(args);

    using var host = builder.Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.CloseAndFlush();
}