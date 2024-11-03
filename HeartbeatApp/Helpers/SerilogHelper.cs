using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace HeartbeatApp.Helpers
{
    public static class SerilogHelper
    {
        public static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    path: "logs/heartbeatapp.log",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .CreateLogger();
        }
    }
}
