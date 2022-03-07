using BabelBot.Shared.Commands;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Storage;

namespace BabelBot.Receiver.Commands;

public class ListUsersCommand : Command
{
    public override string Keyword => "listusers";

    private IUsers _users { get; }

    public ListUsersCommand(IUsers users) => _users = users;


    public override Task<CommandResult> Run(ReceivedMessage _message, IEnumerable<string> _arguments, CancellationToken _)
    {
        var users = _users.GetList();

        return Task.FromResult(new CommandResult($"Registered users: {string.Join(", ", users)}"));
    }
}
