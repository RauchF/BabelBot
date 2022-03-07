namespace BabelBot.Shared.Messenger;

public struct ReceivedMessage
{
    public int Id { get; set; }
    public long? UserId { get; set; }
    public long ChatId { get; set; }
    public string Text { get; set; }
}
