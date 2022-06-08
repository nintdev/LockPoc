using System;
using System.Data;
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

        public NumberRepository(IContext context, IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }
        
        public async Task<ulong> GetNewNumber(string type)
        {
            var schemaName = _configurationProvider.GetString("DatabaseSchema");
            var tableName = _configurationProvider.GetString("TableName");
            
            using (var connection = _context.CreateConnection())
            {
                if(connection.State != ConnectionState.Open)
                    connection.Open();
                
                using(var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    var latestNumber = await connection.ExecuteScalarAsync<ulong>(
                        $"SELECT LastIssuedNumber FROM {schemaName}.{tableName} WITH(XLOCK, ROWLOCK) WHERE Type = '{type}'", transaction: transaction);
                    
                    latestNumber++;

                    try
                    {
                        await connection.ExecuteAsync(
                            $"UPDATE {schemaName}.{tableName} SET LastIssuedNumber = {latestNumber} WHERE Type = '{type}'", transaction: transaction);
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
    }
}