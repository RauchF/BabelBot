using BabelBot.Shared;

namespace BabelBot.Receiver.Telegram;

public class TelegramOptions : IReceiverSettings
{
    public string ApiKey { get; set; } = string.Empty;

    public long[] AllowedUsers { get; set; } = Array.Empty<long>();

    public bool OnlyReactToAllowedUsers { get; set; } = true;
}