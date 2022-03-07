namespace BabelBot.Shared.Messenger;

public interface IMessenger
{
    Task SendTextMessage(long chatId, string text, int? replyToMessageId, CancellationToken cancellationToken = default);
}
