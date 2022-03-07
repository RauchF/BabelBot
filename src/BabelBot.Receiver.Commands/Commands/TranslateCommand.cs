using BabelBot.Shared.Commands;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Translation;

namespace BabelBot.Receiver.Commands;

public class TranslateCommand : Command
{
    public override bool IsDefault => true;
    public override string Keyword => "";

    private ITranslator _translator { get; }
    private IMessenger _messenger { get; }

    public TranslateCommand(ITranslator translator, IMessenger messenger)
    {
        _translator = translator;
        _messenger = messenger;
    }


    public override async Task<CommandResult> Run(
        ReceivedMessage message,
        IEnumerable<string> _arguments,
        CancellationToken cancellationToken)
    {
        var translatedText = await _translator.TranslateAsync(message.Text, cancellationToken);

        foreach (var part in SplitTranslationResult(translatedText))
        {
            await _messenger.SendTextMessage(
                chatId: message.ChatId,
                text: part,
                replyToMessageId: message.Id,
                cancellationToken: cancellationToken);
        }

        return new CommandResult();
    }

    private static IEnumerable<string> SplitTranslationResult(TranslationResult translatedText)
    {
        const int maxPartLength = 4096;
        const int maxParts = 99;

        var length = translatedText.Text.Length;

        if (length < maxPartLength)
        {
            yield return translatedText.Text;
            yield break;
        }

        const int chunkSize = maxPartLength - 10;
        var totalParts = Math.Min(maxParts, (int)Math.Ceiling((double)length / chunkSize));
        for (int i = 0, c = 1; i < length && c < maxParts; i += chunkSize)
        {
            yield return
                $"[{c}/{totalParts}]\n\n{translatedText.Text.Substring(i, Math.Min(chunkSize, length - i))}";
            c++;
        }
    }
}
