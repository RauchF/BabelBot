using BabelBot.Shared.Options;
using BabelBot.Shared.Storage;
using Microsoft.Extensions.Options;

namespace BabelBot.Storage;

public class Users : IUsers
{
    private List<long> UserIds = new();

    public Users(IOptions<TelegramOptions> telegramOptions)
    {
        UserIds.AddRange(telegramOptions.Value.AllowedUsers);
    }

    public IEnumerable<long> GetList()
    {
        return UserIds.AsEnumerable();
    }

    public bool IsValidUser(long id)
    {
        return UserIds.Contains(id);
    }

    public void AddUsers(IEnumerable<long> ids)
    {
        UserIds.AddRange(ids);
    }
}
