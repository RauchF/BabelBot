using BabelBot.Shared.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace BabelBot.Receiver.Commands;

public static class ServiceCollectionExtension
{
    public static void AddCommands(this IServiceCollection collection)
    {
        collection.AddSingleton<ICommandFactory, CommandFactory>();
        collection.AddSingleton<ICommand, ListUsersCommand>();
        collection.AddSingleton<ICommand, AddUsersCommand>();
        collection.AddSingleton<ICommand, DeleteUsersCommand>();
        collection.AddSingleton<ICommand, TranslateCommand>();
    }
}
