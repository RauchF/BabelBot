using BabelBot.Shared.Commands;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Storage;

namespace BabelBot.Receiver.Commands;

public class AddUsersCommand : Command
{
    public override string Keyword => "addusers";

    private IUsers _users { get; }

    public AddUsersCommand(IUsers users) => _users = users;


    public override Task<CommandResult> Run(ReceivedMessage message, IEnumerable<string> arguments, CancellationToken _)
    {
        var ids = arguments
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
