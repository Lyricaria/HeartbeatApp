using HeartbeatApp.Interfaces;
using Quartz;
using Serilog;

namespace HeartbeatApp.Jobs
{
    public class HeartbeatJob : IJob
    {
        private readonly IHeartbeatEvent _heartbeatEvent;

        public HeartbeatJob(IHeartbeatEvent heartbeatEvent)
        {
            _heartbeatEvent = heartbeatEvent;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Log.Debug("HeartbeatJob execution started");
            await _heartbeatEvent.InvokeAsync();
            Log.Debug("HeartbeatJob execution finished");
        }
    }
}
