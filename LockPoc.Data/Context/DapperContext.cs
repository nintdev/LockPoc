using System.Data;
using System.Data.SqlClient;
using LockPoc.Data.Core;

namespace LockPoc.Data.Context
{
    public class DapperContext: IContext
    {
        private readonly IConfigurationProvider _configurationProvider;

        protected virtual string GetConnectionString(string name)
            => _configurationProvider.GetConnectionString(name);
        
        public DapperContext(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(GetConnectionString("Connection"));
        
        public IDbConnection CreateMasterConnection()
            => new SqlConnection(GetConnectionString("MasterConnection"));
    }
}