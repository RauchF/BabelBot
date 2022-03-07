using System.Text.RegularExpressions;
using BabelBot.Shared.Commands;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Storage;
using Microsoft.Extensions.Logging;

namespace BabelBot.Receiver.Commands;

public abstract class Command : ICommand
{
    protected readonly ILogger<Command> _logger;
    protected readonly IUsers _users;

    public Command(ILogger<Command> logger, IUsers users)
    {
        _logger = logger;
        _users = users;
    }
    public virtual bool IsDefault { get; } = false;
    public abstract string Keyword { get; }
    public abstract IEnumerable<UserRole> AllowedRoles { get; }

    public Task<CommandResult> Run(ReceivedMessage message, CancellationToken cancellationToken)
    {
        if (message.UserId is null)
        {
            _logger.LogDebug("Ignored command {Command} because sender is not a user.", Keyword);
            return Task.FromResult(new CommandResult()); // fail silently
        }

        var user = _users.GetUser(message.UserId.Value) ?? new User { Role = UserRole.Anonymous };
        if (!AllowedRoles.Contains(user.Role))
        {
            _logger.LogDebug("Ignored command {Command} because sender does not have sufficient permissions.", Keyword);
            return Task.FromResult(new CommandResult()); // fail silently
        }

        var commandPattern = new Regex($"^/{this.Keyword} ");
        var arguments = commandPattern.Replace(message.Text, "").Split(' ');

        return Run(message, arguments, cancellationToken);
    }

    public abstract Task<CommandResult> Run(ReceivedMessage message, IEnumerable<string> commandArguments, CancellationToken cancellationToken);
}
