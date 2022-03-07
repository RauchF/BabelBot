using BabelBot.Shared.Messenger;

namespace BabelBot.Shared.Commands
{
    /// <summary>
    /// Special command placeholder for unknown commands
    /// </summary>
    public class MissingCommand : ICommand
    {
        public string Keyword { get; }

        public bool IsDefault => false;

        public MissingCommand(string keyword)
        {
            Keyword = keyword;
        }

        public Task<CommandResult> Run(CancellationToken _token, ReceivedMessage _message)
        {
            throw new NotImplementedException();
        }
    }
}
