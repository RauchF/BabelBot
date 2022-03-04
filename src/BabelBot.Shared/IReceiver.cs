namespace BabelBot.Shared;

public interface IReceiver
{
    public Task Start(CancellationToken cts);
}