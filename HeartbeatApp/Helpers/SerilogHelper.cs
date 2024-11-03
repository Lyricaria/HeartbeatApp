using Microsoft.Extensions.Configuration;
using Serilog;

namespace HeartbeatApp.Helpers
{
    public static class SerilogHelper
    {
        public static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
        }
    }
}
