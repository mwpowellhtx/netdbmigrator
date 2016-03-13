namespace Kingdom.Data
{
    /// <summary>
    /// Sql Server primary key or unique constraint interface.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public interface ISqlServerPrimaryKeyOrUniqueConstraint<TParent>
        : IPrimaryKeyOrUniqueConstraint<TParent>
        where TParent : ISqlServerPrimaryKeyOrUniqueConstraint<TParent>
    {
    }

    /// <summary>
    /// Sql Server primary key or unique constraint.
    /// </summary>
    public class SqlServerPrimaryKeyOrUniqueConstraint
        : PrimaryKeyOrUniqueConstraintBase<SqlServerPrimaryKeyOrUniqueConstraint>
            , ISqlServerPrimaryKeyOrUniqueConstraint<SqlServerPrimaryKeyOrUniqueConstraint>
    {
    }
}
