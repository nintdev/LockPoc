using FluentMigrator;
using LockPoc.Data.Core;

namespace LockPoc.Data.Migrations
{
    [Migration(202206020001)]
    public class CreateNumberTable: Migration
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
                .WithColumn("Id").AsInt32().Identity()
                .WithColumn("Type").AsFixedLengthString(64).NotNullable().Unique()
                .WithColumn("LastIssuedNumber").AsInt64().NotNullable()
                .WithColumn("LastIssuedTimestamp").AsCustom("rowversion").NotNullable()
                .WithColumn("LastIssuedUserId").AsInt32().NotNullable();

            Create.PrimaryKey($"PK_{_tableName}_Id_Type").OnTable(_tableName).WithSchema(_databaseSchema)
                .Columns(new string[] { "Id", "Type" });
        }

        public override void Down()
        {
            Delete.Table(_tableName).InSchema(_databaseSchema);
            Delete.Schema(_databaseSchema);
        }
    }
}