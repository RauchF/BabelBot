using BabelBot.Shared.Translation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BabelBot.Translator.DeepL.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDeepLTranslator(this IServiceCollection services, IConfiguration configurationSection)
    {
        services.AddSingleton<ITranslator, DeepLTranslator>();
        services.AddSingleton(s => new global::DeepL.Translator(
            s.GetService<IOptions<DeepLTranslatorOptions>>()!.Value.AuthKey
        ));

        services.Configure<DeepLTranslatorOptions>(configurationSection);
    }
}
