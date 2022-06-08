using FluentMigrator.Runner.VersionTableInfo;

namespace LockPoc.Data
{
#pragma warning disable CS0618
    /// <summary>
    /// This class is used for the VersionInfo table configuration, it sits in this project because of the dependency on
    /// FluentMigrator.Runner which we don't want in another project.
    /// </summary>
    public class VersionInfo: DefaultVersionTableMetaData
#pragma warning restore CS0618
    {
        public override bool OwnsSchema => false;
        public override string SchemaName => "system";
        public override string TableName => "Migrations";
    }
}