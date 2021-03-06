using BabelBot.Shared.Commands;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Options;
using BabelBot.Shared.Storage;
using BabelBot.Shared.Translation;
using BabelBot.Shared.Translation.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BabelBot.Receiver.Commands;

public class TranslateCommand : Command
{
    public override bool IsDefault => true;
    public override string Keyword => string.Empty;
    public override string Description => string.Empty;

    public override IEnumerable<UserRole> AllowedRoles => _telegramOptions.OnlyReactToAllowedUsers
        ? new[] { UserRole.TranslationUser, UserRole.Superuser }
        : Enum.GetValues<UserRole>();

    private ITranslator _translator { get; }
    private IMessenger _messenger { get; }
    private readonly TelegramOptions _telegramOptions;


    public TranslateCommand(
        ILogger<TranslateCommand> logger,
        IUsers users,
        ITranslator translator,
        IMessenger messenger,
        IOptions<TelegramOptions> telegramOptions)
        : base(logger, users)
    {
        _translator = translator;
        _messenger = messenger;
        _telegramOptions = telegramOptions.Value;
    }


    public override async Task<CommandResult> Run(
        ReceivedMessage message,
        IEnumerable<string> commandArguments,
        CancellationToken cancellationToken)
    {
        TranslationResult translationResult;
        try
        {
            translationResult = await _translator.TranslateAsync(message.Text, cancellationToken);
        }
        catch (TranslationFailedException e)
        {
            return new CommandResult($"An error occurred while translating:\n{e.Message}");
        }

        foreach (var part in SplitTranslationResult(translationResult))
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
        // FIXME: 4096 is the maximum length for Telegram messages and should not be set here
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
