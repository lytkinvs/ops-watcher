using OpsWatcher.Web.Models;

namespace OpsWatcher.Web.services;

public class BackgroundStartupHostedService : IHostedService
{
    private int executionCount = 0;
    private readonly ILogger<BackgroundStartupHostedService> _logger;
    private Timer? _timer = null;
    private readonly DataService _dataService;

    public BackgroundStartupHostedService(
        DataService _dataService,
        ILogger<BackgroundStartupHostedService> logger)
    {
        _logger = logger;
        this._dataService = _dataService;
    }

    public async Task StartAsync(CancellationToken stoppingToken)
    {
        using var httpClient = new HttpClient();
        
        var items = await 
            httpClient.GetFromJsonAsync<List<Item>>("http://localhost:8002/parse/list", cancellationToken: stoppingToken);
        
        _dataService.AddItems(items);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
