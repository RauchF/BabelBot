using DeepL;

namespace BabelBot.Translator.DeepL;

public class DeepLTranslatorOptions
{
    public string AuthKey { get; set; } = String.Empty;

    public string DefaultTargetLanguageCode { get; set; } = LanguageCode.EnglishBritish;
}