using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace HeartbeatApp.Helpers
{
    public static class HostBuilderHelper
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = BuildConfiguration();

            //Serilog
            Log.Logger = SerilogHelper.CreateSerilogLogger(configuration);

            // Create the host builder
            var builder = Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // Add appsettings.json to the configuration pipeline
                    config.AddConfiguration(configuration);
                })
                .ConfigureServices(ServiceConfigurator.ConfigureServices);

            return builder;
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            return configuration;
        }
    }
}
