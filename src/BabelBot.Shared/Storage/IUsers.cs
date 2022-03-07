namespace BabelBot.Shared.Storage;

public interface IUsers
{
    IEnumerable<long> GetList();
    void AddUsers(IEnumerable<long> ids);
    void DeleteUsers(IEnumerable<long> ids);
    bool IsValidUser(long id);
}
