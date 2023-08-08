using Microsoft.Extensions.Hosting;

namespace AutoDrivingCarSimulation
{
    public class App : IHostedService
    {
        private readonly IProcess process;
        public App(IProcess process)
        {
            this.process = process;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await process.Start();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
