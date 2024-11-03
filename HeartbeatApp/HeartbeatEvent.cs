using NATS.Client.Core;
using Serilog;
using System.Text;

namespace HeartbeatApp
{
    public class HeartbeatEvent : IAsyncDisposable
    {
        private readonly NatsConnection _connection;
        private readonly string _subject = "game.heartbeat";

        public HeartbeatEvent(string natsUrl)
        {
            var options = NatsOpts.Default with { Url = natsUrl };
            _connection = new NatsConnection(options);
        }

        public async Task StartAsync()
        {
            await _connection.ConnectAsync();
            Log.Information("Connected to NATS server at {NatsUrl}", NatsOpts.Default.Url);
        }

        public async Task InvokeAsync()
        {
            try
            {
                var message = $"Heartbeat at {DateTime.UtcNow:o}";
                var payload = Encoding.UTF8.GetBytes(message);

                await _connection.PublishAsync(_subject, payload);
                Log.Information("Published heartbeat message: {Message}", message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error publishing heartbeat message");
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _connection.DisposeAsync();
            Log.Information("Disconnected from NATS server");
        }
    }
}

