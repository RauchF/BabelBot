namespace BabelBot.Shared.Commands;

public interface ICommandFactory
{
    ICommand GetCommand(string message);
}
