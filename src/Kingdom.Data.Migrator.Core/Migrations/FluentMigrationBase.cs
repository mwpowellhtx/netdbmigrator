namespace Kingdom.Data.Migrations
{
    /// <summary>
    /// Represents fluent database migration.
    /// </summary>
    /// <typeparam name="TAlterTableFluently"></typeparam>
    public abstract class FluentMigrationBase<TAlterTableFluently> : MigrationBase
        where TAlterTableFluently : class, IAlterTableFluently, new()
    {
        /// <summary>
        /// Alter backing field.
        /// </summary>
        private readonly IAlterFluently<TAlterTableFluently> _alterFluently;

        /// <summary>
        /// Gets the fluent Alter instance.
        /// </summary>
        protected IAlterFluently<TAlterTableFluently> Alter
        {
            get { return _alterFluently; }
        }

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="alterFluently"></param>
        protected FluentMigrationBase(IAlterFluently<TAlterTableFluently> alterFluently)
        {
            _alterFluently = alterFluently;
        }
    }
}
