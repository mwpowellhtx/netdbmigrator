namespace Kingdom.Data
{
    /// <summary>
    /// Sql Server Check Constraint interface.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public interface ISqlServerCheckConstraint<out TParent>
        : ICheckConstraint<TParent>
        where TParent : ISqlServerCheckConstraint<TParent>
    {
    }

    /// <summary>
    /// Sql Server Check Constraint concept.
    /// </summary>
    public class SqlServerCheckConstraint
        : CheckConstraintBase<SqlServerCheckConstraint>
    {
    }
}
