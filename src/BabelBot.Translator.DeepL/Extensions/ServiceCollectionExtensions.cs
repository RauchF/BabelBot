using BabelBot.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BabelBot.Translator.DeepL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDeepLTranslator(this IServiceCollection services, IConfiguration configurationSection)
    {
        services.AddSingleton<ITranslator, DeepLTranslator>();
        services.Configure<DeepLTranslatorOptions>(configurationSection);
    }
}