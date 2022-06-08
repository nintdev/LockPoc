using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LockPoc.Data.Context;

namespace LockPoc.Data
{
    public class Database : IDatabase
    {
        private readonly IContext _context;

        public Database(IContext context)
        {
            _context = context;
        }

        public async Task CreateDatabaseAsync(string databaseName)
        {
            using (var connection = _context.CreateMasterConnection())
            {
                var records = await connection.QueryAsync($"SELECT * FROM sys.databases WHERE name = '{databaseName}'");
                if (!records.Any())
                    await connection.ExecuteAsync($"CREATE DATABASE {databaseName}");
            }
        }

        public async Task DropDatabaseAsync(string databaseName)
        {
            using (var connection = _context.CreateMasterConnection())
            {
                var query = $@"
IF EXISTS (SELECT 1 FROM sys.databases WHERE [name] = N'{databaseName}')
BEGIN
    USE [master]
    ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    DROP DATABASE [{databaseName}]
END";
                
                await connection.ExecuteAsync(query);
            }
        }
    }
}