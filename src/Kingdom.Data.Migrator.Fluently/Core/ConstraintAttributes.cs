namespace Kingdom.Data
{
    /// <summary>
    /// Constraint attribute interface.
    /// </summary>
    public interface IConstraintAttribute : IDataAttribute
    {
    }

    /// <summary>
    /// Constraint attribute interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConstraintAttribute<T> : IDataAttribute<T>, IConstraintAttribute
    {
    }

    /// <summary>
    /// Constraint attribute base class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ConstraintAttributeBase<T>
        : DataAttributeBase<T>
            , IConstraintAttribute<T>
    {
    }

    /// <summary>
    /// Clustered constraint attribute.
    /// </summary>
    public interface IClusteredConstraintAttribute : IConstraintAttribute<ClusteredType>
    {
    }

    /// <summary>
    /// Clustered constraint attribute.
    /// </summary>
    public class ClusteredConstraintAttribute
        : ConstraintAttributeBase<ClusteredType>
            , IClusteredConstraintAttribute
    {
    }

    /// <summary>
    /// Table index constraint attribute.
    /// </summary>
    public interface ITableIndexConstraintAttribute : IConstraintAttribute<TableIndexType>
    {
    }

    /// <summary>
    /// Table index constraint attribute.
    /// </summary>
    public class TableIndexConstraintAttribute
        : ConstraintAttributeBase<TableIndexType>
            , ITableIndexConstraintAttribute
    {
    }

    /// <summary>
    /// Foreign key action constraint attribute.
    /// </summary>
    public interface IForeignKeyActionConstraintAttribute : IConstraintAttribute<ForeignKeyAction>
    {

        /// <summary>
        /// Gets or sets the Trigger.
        /// </summary>
        ForeignKeyTrigger Trigger { get; set; }
    }

    /// <summary>
    /// Foreign key action constraint attribute.
    /// </summary>
    public class ForeignKeyActionConstraintAttribute
        : ConstraintAttributeBase<ForeignKeyAction>
            , IForeignKeyActionConstraintAttribute
    {
        /// <summary>
        /// Gets or sets the Trigger.
        /// </summary>
        public ForeignKeyTrigger Trigger { get; set; }
    }
}
