using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
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

        public async Task<IDbConnection> CreateConnectionAsync(string name, bool returnOpened = true)
        {
            var connection = new SqlConnection(GetConnectionString(name));

            if (returnOpened && connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            return connection;
        }
    }
}