namespace Kingdom.Data
{
    /// <summary>
    /// Sql Server primary key or unique constraint interface.
    /// </summary>
    public interface ISqlServerPrimaryKeyOrUniqueConstraint : IPrimaryKeyOrUniqueConstraint
    {
    }

    /// <summary>
    /// Sql Server primary key or unique constraint.
    /// </summary>
    public class SqlServerPrimaryKeyOrUniqueConstraint
        : PrimaryKeyOrUniqueConstraintBase
            , ISqlServerPrimaryKeyOrUniqueConstraint
    {
    }
}
