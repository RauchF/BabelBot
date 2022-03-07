using BabelBot.Shared.Messenger;

namespace BabelBot.Shared.Commands;

public interface ICommand
{
    bool IsDefault { get; }
    string Keyword { get; }
    Task<CommandResult> Run(ReceivedMessage message, CancellationToken cancellationToken);
}
