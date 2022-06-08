using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LockPoc.Data.Context;
using LockPoc.Data.Core;

namespace LockPoc.Data.Repositories
{
    public class NumberRepository: INumberRepository
    {
        private readonly IContext _context;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly string _schemaName;
        private readonly string _tableName;

        public NumberRepository(IContext context, IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
            _schemaName = _configurationProvider.GetString("DatabaseSchema");
            _tableName = _configurationProvider.GetString("TableName");
        }
        
        public async Task<ulong> CreateNewNumber(string type, int userId = 0)
        {
            using (var connection = await _context.CreateConnectionAsync(Constants.ConnectionNames.Connection))
            {
                using(var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    var latestNumber = await connection.ExecuteScalarAsync<ulong>(
                        $"SELECT LastIssuedNumber FROM {_schemaName}.{_tableName} WITH(XLOCK, ROWLOCK) WHERE Type = '{type}'", transaction: transaction);
                    
                    latestNumber++;

                    try
                    {
                        await connection.ExecuteAsync(
                            $"UPDATE {_schemaName}.{_tableName} SET LastIssuedNumber = {latestNumber}, LastIssuedTimestamp = GETDATE(), LastIssuedUserId = {userId} WHERE Type = '{type}'", transaction: transaction);
                    }
                    catch(Exception e)
                    {
                        transaction.Rollback();
                        // log
                        throw;
                    }
                    
                    transaction.Commit();
                
                    return latestNumber;
                }
            }
        }

        public async Task<ulong> GetLastNumber(string type)
        {
            using (var connection = await _context.CreateConnectionAsync(Constants.ConnectionNames.Connection))
            {
                return await connection.ExecuteScalarAsync<ulong>(
                    $"SELECT LastIssuedNumber FROM {_schemaName}.{_tableName} WITH(XLOCK, ROWLOCK) WHERE Type = '{type}'");
            }
        }
    }
}