using System;
using FluentMigrator;
using FluentMigrator.SqlServer;
using LockPoc.Data.Core;
using LockPoc.Data.Entities;

namespace LockPoc.Data.Migrations
{
    [Migration(202206020002)]
    public class SeedNumberTable: AutoReversingMigration
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly string _databaseSchema;
        private readonly string _tableName;

        public SeedNumberTable(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            _databaseSchema = _configurationProvider.GetString("DatabaseSchema");
            _tableName = _configurationProvider.GetString("TableName");
        }
        
        public override void Up()
        {
            Insert.IntoTable(_tableName).InSchema(_databaseSchema)
                .WithIdentityInsert()
                .Row(new Number
                {
                    Id = 1,
                    Type = "SALESDOCUMENT",
                    LastIssuedNumber = _configurationProvider.GetValue<ulong>("SalesdocumentLastIssuedId"),
                    LastIssuedUserId = 0
                });
            
            Insert.IntoTable(_tableName).InSchema(_databaseSchema)
                .WithIdentityInsert()
                .Row(new Number
                {
                    Id = 2,
                    Type = "INVOICE",
                    LastIssuedNumber = _configurationProvider.GetValue<ulong>("InvoiceLastIssuedId"),
                    LastIssuedUserId = 0
                });
        }
    }
}