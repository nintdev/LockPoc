using System.Data;
using System.Threading.Tasks;

namespace LockPoc.Data.Context
{
    public interface IContext
    {
        Task<IDbConnection> CreateConnectionAsync(string connectionName, bool returnOpened = true);
    }
}