using BabelBot.Shared.Commands;
using BabelBot.Shared.Messenger;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BabelBot.Receiver.Telegram;

public class CommandRegistrator : ICommandRegistrator
{
    ITelegramBotClient _botClient;
    IEnumerable<ICommand> _commands;

    public CommandRegistrator(ITelegramBotClient botClient, IEnumerable<ICommand> commands)
    {
        _botClient = botClient;
        _commands = commands;
    }

    public async Task RegisterCommands(CancellationToken cancellationToken)
    {
        var commands = _commands
            .Where(command => !string.IsNullOrEmpty(command.Keyword))
            .Select(command => new BotCommand { Command = command.Keyword, Description = command.Description });
        await _botClient.SetMyCommandsAsync(commands, cancellationToken: cancellationToken);
    }
}
