using System.Text.RegularExpressions;
using BabelBot.Shared.Commands;
using BabelBot.Shared.Messenger;

namespace BabelBot.Receiver.Commands;

public abstract class Command : ICommand
{
    public virtual bool IsDefault { get; } = false;
    public abstract string Keyword { get; }

    public Task<CommandResult> Run(ReceivedMessage message, CancellationToken cancellationToken)
    {
        var commandPattern = new Regex($"^/{this.Keyword}");
        var arguments = commandPattern.Replace(message.Text, "").Split(' ');

        return Run(message, arguments, cancellationToken);
    }

    public abstract Task<CommandResult> Run(ReceivedMessage message, IEnumerable<string> commandArguments, CancellationToken cancellationToken);
}
