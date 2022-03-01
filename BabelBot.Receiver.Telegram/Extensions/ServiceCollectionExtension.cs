using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BabelBot.Receiver.Telegram.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddTelegramReceiver(this IServiceCollection collection, IConfiguration configurationSection)
    {
        collection.AddSingleton<TelegramReceiver>();
        collection.Configure<TelegramOptions>(configurationSection);
    }
}