using BabelBot.Shared.Options;
using BabelBot.Shared.Storage;
using Microsoft.Extensions.Options;

namespace BabelBot.Storage;

public class Users : IUsers
{
    private List<User> UserStorage = new();

    public Users(IOptions<TelegramOptions> telegramOptions)
    {
        var superusers = telegramOptions.Value.AllowedUsers
            .Select(id => new User { Id = id, Role = UserRole.Superuser });
        UserStorage.AddRange(superusers);
    }

    public IEnumerable<User> GetList()
    {
        return UserStorage.AsEnumerable();
    }

    public IEnumerable<User> GetList(Func<User, bool> predicate)
    {
        return UserStorage.Where(predicate).AsEnumerable();
    }

    public User? GetUser(long id)
    {
        return UserStorage.FirstOrDefault(user => user.Id == id);
    }

    public void AddTranslationUsers(IEnumerable<long> ids)
    {
        var users = ids.Select(id => new User { Id = id, Role = UserRole.TranslationUser });
        UserStorage.AddRange(users);
    }

    public void DeleteTranslationUsers(IEnumerable<long> ids)
    {
        UserStorage.RemoveAll(user => ids.Contains(user.Id));
    }
}
