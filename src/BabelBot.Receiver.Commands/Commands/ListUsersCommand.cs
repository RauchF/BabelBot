using BabelBot.Shared.Commands;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Storage;

namespace BabelBot.Receiver.Commands;

public class ListUsersCommand : ICommand
{
    public bool IsDefault => false;
    public string Keyword => "listusers";

    private IUsers _users { get; }

    public ListUsersCommand(IUsers users) => _users = users;


    public Task<CommandResult> Run(CancellationToken _token, ReceivedMessage _message)
    {
        var users = _users.GetList();

        return Task.FromResult(new CommandResult($"Registered users: {string.Join(", ", users)}"));
    }
}
