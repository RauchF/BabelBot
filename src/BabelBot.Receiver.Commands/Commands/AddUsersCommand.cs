using BabelBot.Shared.Commands;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Storage;
using Microsoft.Extensions.Logging;

namespace BabelBot.Receiver.Commands;

public class AddUsersCommand : Command
{
    public AddUsersCommand(ILogger<AddUsersCommand> logger, IUsers users) : base(logger, users)
    {
    }

    public override string Keyword => "addusers";

    public override IEnumerable<UserRole> AllowedRoles => new[] { UserRole.Superuser };

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

        _users.AddTranslationUsers(ids);

        return Task.FromResult(new CommandResult()
        {
            SuccessMessage = $"The following ids were successfully added: {string.Join(", ", ids)}"
        });
    }
}
