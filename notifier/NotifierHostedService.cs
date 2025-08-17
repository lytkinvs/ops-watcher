namespace OpsWatcher.Notifier;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class NotifierHostedService : IHostedService, IAsyncDisposable
{
    private readonly ILogger<NotifierHostedService> _logger;
    private IConnection _connection;
    private IChannel _channel;
    private INotifier _notifier;

    public NotifierHostedService(ILogger<NotifierHostedService> logger, INotifier notifier)
    {
        _logger = logger;
        _notifier = notifier;
    }

    private async Task InitializeRabbitMQ()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(queue: "notifications",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] {message}");
            await _notifier.NotifyAsync(message);
        };

        await _channel.BasicConsumeAsync("notifications", autoAck: true, consumer: consumer);
    }

    public async Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        await InitializeRabbitMQ();
    }


    public async Task StopAsync(CancellationToken stoppingToken)
    {
        _channel.CloseAsync();
        _connection.CloseAsync();
    }
    

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
        await _channel.DisposeAsync();
    }
}