using System;
using FluentMigrator;
using FluentMigrator.SqlServer;
using LockPoc.Data.Core;

namespace LockPoc.Data.Migrations
{
    [Migration(202206020001)]
    public class CreateNumberTable: AutoReversingMigration
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly string _databaseSchema;
        private readonly string _tableName;

        public CreateNumberTable(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            _databaseSchema = _configurationProvider.GetString("DatabaseSchema");
            _tableName = _configurationProvider.GetString("TableName");
        }
        
        public override void Up()
        {
            Create.Schema(_databaseSchema);
            Create.Table(_tableName).InSchema(_databaseSchema)
                .WithColumn("Id").AsInt32().Identity(1, 1).PrimaryKey()
                .WithColumn("Type").AsFixedLengthString(64).NotNullable().Unique()
                .WithColumn("LastIssuedNumber").AsInt64().NotNullable()
                .WithColumn("LastIssuedTimestamp").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
                .WithColumn("LastIssuedUserId").AsInt32().NotNullable()
                .WithColumn("Version").AsCustom("rowversion").NotNullable();
        }
    }
}