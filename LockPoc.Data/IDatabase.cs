using System.Threading.Tasks;

namespace LockPoc.Data
{
    public interface IDatabase
    {
        Task CreateDatabaseAsync(string databaseName);
        Task DropDatabaseAsync(string databaseName);
    }
}