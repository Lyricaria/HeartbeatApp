namespace HeartbeatApp.Interfaces
{
    public interface IHeartbeatEvent : IAsyncDisposable
    {
        Task StartAsync();
        Task InvokeAsync();
    }
}
