namespace BabelBot.Shared.Messenger;

public interface IReceiver
{
    public Task Start(CancellationToken cts);
}
