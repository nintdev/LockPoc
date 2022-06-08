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
                .Row(new Number
                {
                    Id = 1,
                    Type = Constants.DocumentTypes.SalesDocument,
                    LastIssuedTimestamp = DateTime.Now
                })
                .WithIdentityInsert();

            Insert.IntoTable(_tableName).InSchema(_databaseSchema)
                .Row(new Number
                {
                    Id = 2,
                    Type = Constants.DocumentTypes.Invoice,
                    LastIssuedTimestamp = DateTime.Now
                })
                .WithIdentityInsert();
        }
    }
}