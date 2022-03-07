using BabelBot.Shared.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace BabelBot.Storage;

public static class ServiceCollectionExtension
{
    public static void AddStorage(this IServiceCollection collection)
    {
        collection.AddSingleton<IUsers, Users>();
    }
}
