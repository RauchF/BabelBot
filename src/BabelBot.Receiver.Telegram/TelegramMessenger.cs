using BabelBot.Shared.Messenger;
using Telegram.Bot;

namespace BabelBot.Receiver.Telegram;

public class TelegramMessenger : IMessenger
{
    private ITelegramBotClient _botClient;

    public TelegramMessenger(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task SendTextMessage(long chatId, string text, int? replyToMessageId, CancellationToken cancellationToken = default)
    {
        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: text,
            replyToMessageId: replyToMessageId,
            cancellationToken: cancellationToken);
    }
}
