namespace BabelBot.Shared.Translation.Exceptions;

public class TranslationFailedException : Exception
{
    public TranslationFailedException(string message) : base(message) { }
}
