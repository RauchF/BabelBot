namespace BabelBot.Shared.Commands;

public class CommandResult
{
    public CommandResult() => Error = null;

    public CommandResult(string error) => Error = error;
    public string? Error { get; private set; }

    public bool Success => Error == null;

    public string? SuccessMessage { get; init; } = null;

    public bool HasError => !Success;
}
