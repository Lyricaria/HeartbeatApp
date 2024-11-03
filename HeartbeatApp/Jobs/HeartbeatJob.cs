using Quartz;

namespace HeartbeatApp.Jobs
{
    public class HeartbeatJob : IJob
    {
        private readonly HeartbeatEvent _heartbeatEvent;

        public HeartbeatJob(HeartbeatEvent heartbeatEvent)
        {
            _heartbeatEvent = heartbeatEvent;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _heartbeatEvent.InvokeAsync();
        }
    }
}
