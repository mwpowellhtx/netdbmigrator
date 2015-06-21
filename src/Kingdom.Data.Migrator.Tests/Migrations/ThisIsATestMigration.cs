using Kingdom.Data.Attributes;
using Kingdom.Data.Migrations;

namespace Kingdom.Data.Migrator.Tests.Migrations
{
    /// <summary>
    /// This is a test migration.
    /// </summary>
    [VersionMigration(1, 0)]
    public class ThisIsATestMigration : MigrationBase
    {
        public override void Up()
        {
            // Does not have to do anything here. Just exist and that triggers a migration.
        }

        public override void Down()
        {
            // Does not have to do anything here. Just exist and that triggers a migration.
        }
    }
}
