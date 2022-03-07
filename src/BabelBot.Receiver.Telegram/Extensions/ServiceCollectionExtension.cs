using BabelBot.Shared.Messenger;
using BabelBot.Shared.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace BabelBot.Receiver.Telegram.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddTelegramReceiver(this IServiceCollection collection, IConfiguration configurationSection)
    {
        collection.AddSingleton<TelegramReceiver>();
        collection.AddSingleton<IMessenger, TelegramMessenger>();
        collection.AddSingleton<ITelegramBotClient, TelegramBotClient>((services) =>
            new TelegramBotClient(services.GetService<IOptions<TelegramOptions>>()!.Value.ApiKey));

        collection.Configure<TelegramOptions>(configurationSection);
    }
}
