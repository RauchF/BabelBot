using BabelBot.Receiver.Telegram;
using BabelBot.Shared;
using Microsoft.Extensions.Options;

namespace BabelBot.Worker.Factory;

public class ReceiverFactory
{
    private WorkerOptions _options;

    private IServiceProvider _provider;

    public ReceiverFactory(IOptions<WorkerOptions> options, IServiceProvider provider)
    {
        _options = options.Value;
        _provider = provider;
    }

    public IEnumerable<IReceiver> CreateAllConfigured()
    {
        return _options.Receivers
            .Select(x => x switch
            {
                "Telegram" => typeof(TelegramReceiver),
                _ => throw new ArgumentException($"Requested Receiver [{x}] could not be resolved")
            })
            .Select(t => ActivatorUtilities.GetServiceOrCreateInstance(_provider, t))
            .Cast<IReceiver>();
    }
}