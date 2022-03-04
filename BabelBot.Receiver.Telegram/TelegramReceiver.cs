using BabelBot.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BabelBot.Receiver.Telegram;

public class TelegramReceiver : IReceiver
{
    private readonly ILogger<TelegramReceiver> _logger;
    private readonly ITranslator _translator;
    private readonly TelegramOptions _options;
    private readonly TelegramBotClient _botClient;

    public TelegramReceiver(ILogger<TelegramReceiver> logger, IOptions<TelegramOptions> options, ITranslator translator)
    {
        _logger = logger;
        _translator = translator;
        _options = options.Value;
        _botClient = new TelegramBotClient(_options.ApiKey);
    }

    public Task Start(CancellationToken cts)
    {
        _botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, null, cts);
        
        return Task.CompletedTask;
    }
    
    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message)
        {
            _logger.LogInformation("Received Update of unhandled type {Type}", update.Type);
            return;
        }

        if (update.Message is null)
        {
            _logger.LogWarning("Received a Message type Update without a Message");
            return;
        }

        if (update.Message!.Type != MessageType.Text)
        {
            _logger.LogInformation("Received Message of unhandled type {Type}", update.Message.Type);
            return;
        }

        var chatId = update.Message.Chat.Id;
        var sourceText = update.Message.Text;

        if (sourceText is null)
            return;

        _logger.LogDebug("Received message update {Update} from {From} in chat {Chat}", update.Id, update.Message.From, chatId);

        if (_options.OnlyReactToAllowedUsers && !_options.AllowedUsers.Contains(update.Message.From!.Id))
        {
            _logger.LogDebug("Ignored message update {Update} because sender is not in the list of AllowedUsers", update.Id);
            return;
        }

        var translatedText = await _translator.TranslateAsync(sourceText, cancellationToken);

        foreach (var part in SplitTranslationResult(translatedText))
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId, 
                text: part,
                replyToMessageId: update.Message.MessageId,
                cancellationToken: cancellationToken);
        }
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

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case ApiRequestException apiRequestException:
                _logger.LogError("Telegram API Error: [{ErrorCode}] {ErrorMessage}", apiRequestException.ErrorCode, apiRequestException.Message);
                break;
            default:
                _logger.LogError("Unhandled Exception: {ErrorMessage}", exception.ToString());
                break;
        }

        return Task.CompletedTask;
    }
}