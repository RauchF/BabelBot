using BabelBot.Shared.Translation;
using DeepL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BabelBot.Translator.DeepL;

public class DeepLTranslator : ITranslator
{
    private readonly ILogger<DeepLTranslator> _logger;
    private readonly DeepLTranslatorOptions _options;

    private readonly global::DeepL.Translator _translator;

    public DeepLTranslator(ILogger<DeepLTranslator> logger, IOptions<DeepLTranslatorOptions> options)
    {
        _logger = logger;
        _options = options.Value;
        _translator = new global::DeepL.Translator(_options.AuthKey);
    }

    public async Task<TranslationResult> TranslateAsync(string text, TranslationContext context, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Translating message of length {Length} from {SourceLanguage} to {TargetLanguage}", text.Length, context.SourceLanguage, context.TargetLanguage);
        var translation = await _translator.TranslateTextAsync(text, context.SourceLanguage, context.TargetLanguage ?? LanguageCode.EnglishBritish,
            null, cancellationToken);

        return new TranslationResult()
        {
            DetectedSourceLanguage = translation.DetectedSourceLanguageCode,
            Text = translation.Text
        };
    }

    public Task<TranslationResult> TranslateAsync(string text, CancellationToken cancellationToken)
    {
        return TranslateAsync(
            text,
            new TranslationContext { TargetLanguage = _options.DefaultTargetLanguageCode },
            cancellationToken);
    }
}
