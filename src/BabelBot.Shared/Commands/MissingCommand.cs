using BabelBot.Shared.Messenger;
using BabelBot.Shared.Storage;

namespace BabelBot.Shared.Commands
{
    /// <summary>
    /// Special command placeholder for unknown commands
    /// </summary>
    public class MissingCommand : ICommand
    {
        public string Keyword { get; }

        public bool IsDefault => false;

        public IEnumerable<UserRole> AllowedRoles => Enum.GetValues<UserRole>();

        public MissingCommand(string keyword)
        {
            Keyword = keyword;
        }

        public Task<CommandResult> Run(ReceivedMessage _message, CancellationToken _token)
        {
            throw new NotImplementedException();
        }
    }
}
