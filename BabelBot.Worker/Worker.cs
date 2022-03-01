using System.Reflection;
using BabelBot.Shared;
using BabelBot.Worker.Factory;

namespace BabelBot.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly IEnumerable<IReceiver> _receivers;

    private readonly WorkerOptions _options;

    public Worker(ILogger<Worker> logger, IConfiguration configuration, ReceiverFactory receiverFactory)
    {
        _logger = logger;

        _options = new WorkerOptions();
        configuration.GetSection(WorkerOptions.SectionKey).Bind(_options);

        _receivers = receiverFactory.CreateAllConfigured();
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var receiver in _receivers)
        {
            _logger.LogInformation("Starting receiver {Receiver}", receiver.GetType().Name);
            await receiver.Start(stoppingToken);
        }
        
        while (!stoppingToken.IsCancellationRequested)
        {
            // _logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}