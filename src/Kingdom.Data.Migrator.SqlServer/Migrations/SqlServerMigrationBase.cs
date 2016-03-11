namespace Kingdom.Data.Migrations
{
    /// <summary>
    /// Sql server fluent migration base.
    /// </summary>
    public abstract class SqlServerFluentMigrationBase : FluentMigrationBase<SqlServerFluentAlterTable>
    {
        /// <summary>
        /// Protected constructor.
        /// </summary>
        protected SqlServerFluentMigrationBase()
            : base(new FluentAlterRoot<SqlServerFluentAlterTable>())
        {
        }
    }
}
