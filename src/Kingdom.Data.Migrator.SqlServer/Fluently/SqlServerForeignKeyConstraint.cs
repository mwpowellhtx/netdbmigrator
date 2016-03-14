namespace Kingdom.Data
{
    /// <summary>
    /// Sql Server Foreign Key constraint interface.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public interface ISqlServerForeignKeyConstraint<out TParent>
        : IForeignKeyConstraint<TParent>
        where TParent : ISqlServerForeignKeyConstraint<TParent>
    {
    }

    /// <summary>
    /// Sql Server primary key or unique constraint.
    /// </summary>
    public class SqlServerForeignKeyConstraint
        : ForeignKeyConstraintBase<SqlServerForeignKeyConstraint>
            , ISqlServerForeignKeyConstraint<SqlServerForeignKeyConstraint>
    {
    }
}
