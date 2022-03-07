using BabelBot.Shared.Commands;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Storage;
using Microsoft.Extensions.Logging;

namespace BabelBot.Receiver.Commands;

public class ListUsersCommand : Command
{
    public ListUsersCommand(ILogger<ListUsersCommand> logger, IUsers users) : base(logger, users)
    {
    }

    public override string Keyword => "listusers";

    public override IEnumerable<UserRole> AllowedRoles => new[] { UserRole.Superuser };

    public override Task<CommandResult> Run(ReceivedMessage _message, IEnumerable<string> _arguments, CancellationToken _)
    {
        var users = _users.GetList().Select(user => user.Id);

        return Task.FromResult(new CommandResult($"Registered users: {string.Join(", ", users)}"));
    }
}
