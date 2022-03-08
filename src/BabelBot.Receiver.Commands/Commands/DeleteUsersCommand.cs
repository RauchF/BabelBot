using BabelBot.Shared.Commands;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Storage;
using Microsoft.Extensions.Logging;

namespace BabelBot.Receiver.Commands;

public class DeleteUsersCommand : Command
{
    public DeleteUsersCommand(ILogger<DeleteUsersCommand> logger, IUsers users) : base(logger, users) { }

    public override string Keyword => "deleteusers";
    public override string Description =>
        $@"Remove one or more users from the list of users BabelBot translates for: ""/{Keyword} <id1> <id2>"".";

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

        var users = _users.GetList(user => ids.Contains(user.Id) && user.Role != UserRole.Superuser);

        _users.DeleteTranslationUsers(users.Select(user => user.Id));

        return Task.FromResult(new CommandResult()
        {
            SuccessMessage = $"The following ids will no longer receive translations: {string.Join(", ", ids)}"
        });
    }
}
