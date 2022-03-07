namespace BabelBot.Shared.Messenger;

public interface ICommandRegistrator
{
    Task RegisterCommands(CancellationToken cancellationToken);
}
