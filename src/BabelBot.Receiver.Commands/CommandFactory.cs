using System.Text.RegularExpressions;
using BabelBot.Shared.Commands;

namespace BabelBot.Receiver.Commands;

public class CommandFactory : ICommandFactory
{
    private static readonly Regex CommandExpression = new Regex("\\/(?<command>[a-z]+)(?: .*?)?$");
    private IEnumerable<ICommand> _commands;

    public CommandFactory(IEnumerable<ICommand> commands) => _commands = commands;

    public ICommand GetCommand(string message)
    {
        var keyword = ExtractCommandKeyword(message);
        if (keyword is null)
        {
            return _commands.FirstOrDefault(cmd => cmd.IsDefault) ?? throw new NoDefaultCommandException();
        }

        var command = _commands.FirstOrDefault(cmd => cmd.Keyword == keyword);

        if (command is null)
        {
            return new MissingCommand(keyword);
        }

        return command!;
    }

    private string? ExtractCommandKeyword(string message)
    {
        var match = CommandExpression.Match(message);
        var command = match?.Groups["command"]?.Value;

        return command != string.Empty ? command : null;
    }
}
