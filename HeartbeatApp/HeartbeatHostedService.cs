using HeartbeatApp.Interfaces;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace HeartbeatApp
{
    public class HeartbeatHostedService : IHostedService
    {
        private readonly IHeartbeatEvent _heartbeatEvent;

        public HeartbeatHostedService(IHeartbeatEvent heartbeatEvent)
        {
            _heartbeatEvent = heartbeatEvent;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _heartbeatEvent.StartAsync();
            Log.Information("HeartbeatHostedService started");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _heartbeatEvent.DisposeAsync();
            Log.Information("HeartbeatHostedService stopped");
        }
    }
}
