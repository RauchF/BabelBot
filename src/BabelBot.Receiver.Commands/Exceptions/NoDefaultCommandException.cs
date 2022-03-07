namespace BabelBot.Receiver.Commands;

public class NoDefaultCommandException : Exception
{
    public NoDefaultCommandException() : base("No default command set up") { }
}
