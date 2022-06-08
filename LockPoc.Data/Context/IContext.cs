using System.Data;

namespace LockPoc.Data.Context
{
    public interface IContext
    {
        IDbConnection CreateConnection();
        IDbConnection CreateMasterConnection();
    }
}