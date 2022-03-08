using BabelBot.Shared.Commands;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Options;
using BabelBot.Shared.Translation;
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
    private readonly ITelegramBotClient _botClient;
    private readonly ICommandFactory _commandFactory;

    public TelegramReceiver(
        ILogger<TelegramReceiver> logger,
        IOptions<TelegramOptions> options,
        ITranslator translator,
        ITelegramBotClient botClient,
        ICommandFactory commandFactory)
    {
        _logger = logger;
        _translator = translator;
        _options = options.Value;
        _botClient = botClient;
        _commandFactory = commandFactory;
    }

    public Task Start(CancellationToken cts)
    {
        _botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: cts);

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
        var messageId = update.Message.MessageId;

        if (sourceText is null or "")
        {
            return;
        }

        _logger.LogDebug("Received message update {Update} from {From} in chat {Chat}", update.Id, update.Message.From, chatId);

        var command = _commandFactory.GetCommand(sourceText);
        if (command is MissingCommand)
        {
            _logger.LogDebug($"Error running command \"{command.Keyword}\": Command not implemented");
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: $"Unknown command: \"{command.Keyword}\"",
                parseMode: ParseMode.Markdown,
                replyToMessageId: messageId,
                allowSendingWithoutReply: true,
                cancellationToken: cancellationToken);
            return;
        }

        var message = new ReceivedMessage { ChatId = chatId, Id = messageId, UserId = update.Message?.From?.Id, Text = sourceText };
        var result = await command.Run(message, cancellationToken);

        if (result.HasError)
        {
            _logger.LogDebug($"Error running command \"{command.Keyword}\": ", result.Error);
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: result.Error!,
                parseMode: ParseMode.Markdown,
                replyToMessageId: messageId,
                allowSendingWithoutReply: true,
                cancellationToken: cancellationToken);
            return;
        }

        if (result.SuccessMessage is not null)
        {
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: result.SuccessMessage,
                parseMode: ParseMode.Markdown,
                replyToMessageId: messageId,
                allowSendingWithoutReply: true,
                cancellationToken: cancellationToken);
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
