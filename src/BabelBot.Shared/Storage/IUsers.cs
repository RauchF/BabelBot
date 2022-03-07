namespace BabelBot.Shared.Storage;

public interface IUsers
{
    IEnumerable<User> GetList();
    IEnumerable<User> GetList(Func<User, bool> predicate);
    void AddTranslationUsers(IEnumerable<long> ids);
    void DeleteTranslationUsers(IEnumerable<long> ids);
    User? GetUser(long id);
}
