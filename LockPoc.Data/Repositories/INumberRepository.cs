using System.Threading.Tasks;

namespace LockPoc.Data.Repositories
{
    public interface INumberRepository
    {
        Task<ulong> GetNewNumber(string type);
    }
}