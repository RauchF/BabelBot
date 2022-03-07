using BabelBot.Shared.Commands;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Storage;

namespace BabelBot.Receiver.Commands;

public class AddUsersCommand : ICommand
{
    public bool IsDefault => false;
    public string Keyword => "addusers";

    private IUsers _users { get; }

    public AddUsersCommand(IUsers users) => _users = users;


    public Task<CommandResult> Run(CancellationToken _, ReceivedMessage message)
    {
        var ids = message.Text
            .Split(' ')
            .Skip(1)
            .Select(id => long.TryParse(id, out var parsed) ? parsed : 0)
            .Where(id => id > 0);

        if (!ids.Any())
        {
            return Task.FromResult(
                new CommandResult(@$"Please provide at least one Telegram user id: ""/{Keyword} <id1> [...<idN>]"" "));
        }

        _users.AddUsers(ids);

        return Task.FromResult(new CommandResult()
        {
            SuccessMessage = $"The following ids were successfully added: {string.Join(", ", ids)}"
        });
    }
}
