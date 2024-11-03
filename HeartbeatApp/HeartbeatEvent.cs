using NATS.Client.Core;
using System.Text;

namespace HeartbeatApp
{
    public class HeartbeatEvent : IAsyncDisposable
    {
        private readonly NatsConnection _connection;
        private readonly string _subject = "game.heartbeat";

        public HeartbeatEvent(string natsUrl)
        {
            // Initialize the NATS connection options
            var options = NatsOpts.Default with { Url = natsUrl };

            // Create the NATS connection
            _connection = new NatsConnection(options);
        }

        public async Task StartAsync()
        {
            await _connection.ConnectAsync();
        }

        public async Task InvokeAsync()
        {
            var message = $"Heartbeat at {DateTime.UtcNow:o}";

            var payload = Encoding.UTF8.GetBytes(message);

            await _connection.PublishAsync(_subject, payload);

            Console.WriteLine($"Published heartbeat message: {message}");
        }

        public async ValueTask DisposeAsync()
        {
            await _connection.DisposeAsync();
        }
    }
}

