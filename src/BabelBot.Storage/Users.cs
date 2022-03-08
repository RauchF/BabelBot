using BabelBot.Shared.Options;
using BabelBot.Shared.Storage;
using Microsoft.Extensions.Options;

namespace BabelBot.Storage;

public class Users : IUsers
{
    private List<User> _userStorage = new();

    public Users(IOptions<TelegramOptions> telegramOptions)
    {
        var superusers = telegramOptions.Value.AllowedUsers
            .Select(id => new User { Id = id, Role = UserRole.Superuser });
        _userStorage.AddRange(superusers);
    }

    public IEnumerable<User> GetList()
    {
        return _userStorage.AsEnumerable();
    }

    public IEnumerable<User> GetList(Func<User, bool> predicate)
    {
        return _userStorage.Where(predicate).AsEnumerable();
    }

    public User? GetUser(long id)
    {
        return _userStorage.FirstOrDefault(user => user.Id == id);
    }

    public void AddTranslationUsers(IEnumerable<long> ids)
    {
        var users = ids.Select(id => new User { Id = id, Role = UserRole.TranslationUser });
        _userStorage.AddRange(users);
    }

    public void DeleteTranslationUsers(IEnumerable<long> ids)
    {
        _userStorage.RemoveAll(user => ids.Contains(user.Id));
    }
}
