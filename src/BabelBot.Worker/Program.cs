using BabelBot.Receiver.Commands;
using BabelBot.Receiver.Telegram.Extensions;
using BabelBot.Storage;
using BabelBot.Translator.DeepL.Extensions;
using BabelBot.Worker.Factory;

namespace BabelBot.Worker;

class Program
{
    private static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        var host = builder.ConfigureServices((hostContext, services) =>
            {
                var configuration = hostContext.Configuration;
                var workerOptionsSection = configuration.GetSection(WorkerOptions.SectionKey);
                services.Configure<WorkerOptions>(workerOptionsSection);

                var telegramOptions = configuration.GetSection("Telegram");
                services.AddTelegramReceiver(telegramOptions);

                var translator = workerOptionsSection.GetValue<string>("Translator");
                switch (translator)
                {
                    case "DeepL":
                        services.AddDeepLTranslator(configuration.GetSection("DeepL"));
                        break;
                    default:
                        throw new ArgumentException($"Unknown translator [{translator}]");
                }

                services.AddCommands();
                services.AddStorage();

                services.AddSingleton<ReceiverFactory>();
                services.AddHostedService<Worker>();
            })
            .Build();

        await host.RunAsync();
    }
}
