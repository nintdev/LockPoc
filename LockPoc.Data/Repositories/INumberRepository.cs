using System.Threading;
using System.Threading.Tasks;

namespace LockPoc.Data.Repositories
{
    public interface INumberRepository
    {
        Task<ulong> CreateNewNumber(string type, int userId = 0);

        Task<ulong> GetLastNumber(string type);
    }
}