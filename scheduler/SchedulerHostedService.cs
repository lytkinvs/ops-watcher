namespace OpsWatcher.Scheduler;

public class ScheduleHostedService : IHostedService, IDisposable
{
    private int executionCount = 0;
    private readonly ILogger<ScheduleHostedService> _logger;
    private Timer? _timer = null;
    private readonly IServiceScopeFactory _serviceScope;

    public ScheduleHostedService(
        IServiceScopeFactory serviceScope, 
        ILogger<ScheduleHostedService> logger)
    {
        _logger = logger;
        _serviceScope = serviceScope;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        Interlocked.Increment(ref executionCount);
        

        _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", executionCount);
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}