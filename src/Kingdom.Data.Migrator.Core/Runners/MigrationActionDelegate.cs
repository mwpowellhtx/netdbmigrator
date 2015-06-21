using Kingdom.Data.Migrations;

namespace Kingdom.Data.Runners
{
    /// <summary>
    /// Delegated migration action.
    /// </summary>
    /// <param name="migration"></param>
    internal delegate void MigrationActionDelegate(IMigration migration);
}
