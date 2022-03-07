using BabelBot.Shared.Messenger;
using BabelBot.Shared.Storage;

namespace BabelBot.Shared.Commands;

public interface ICommand
{
    bool IsDefault { get; }
    string Keyword { get; }
    string Description { get; }
    IEnumerable<UserRole> AllowedRoles { get; }
    Task<CommandResult> Run(ReceivedMessage message, CancellationToken cancellationToken);
}
