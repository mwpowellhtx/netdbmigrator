using Kingdom.Data.Migrations;

namespace Kingdom.Data.Runners
{
    internal delegate void MigrationActionDelegate(AbstractMigration migration);
}
