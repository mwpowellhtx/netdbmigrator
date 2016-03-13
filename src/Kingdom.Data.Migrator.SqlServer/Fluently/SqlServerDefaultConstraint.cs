namespace Kingdom.Data
{
    /// <summary>
    /// Sql Server Default Constraint interface.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public interface ISqlServerDefaultConstraint<out TParent>
        : IDefaultConstraint<TParent>
        where TParent : ISqlServerDefaultConstraint<TParent>
    {
    }

    /// <summary>
    /// Sql Server Default Constraint concept.
    /// </summary>
    public class SqlServerDefaultConstraint
        : DefaultConstraintBase<SqlServerDefaultConstraint>
    {
    }
}
