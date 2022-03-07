using BabelBot.Shared.Messenger;

namespace BabelBot.Shared.Options;

public class TelegramOptions : IReceiverSettings
{
    public string ApiKey { get; set; } = string.Empty;

    public long[] AllowedUsers { get; set; } = Array.Empty<long>();

    public bool OnlyReactToAllowedUsers { get; set; } = true;
}
