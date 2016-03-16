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
    /// Constraint attribute concept.
    /// </summary>
    public abstract class ConstraintAttributeBase : DataAttributeBase, IConstraintAttribute
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
        /// <summary>
        /// Default constructor.
        /// </summary>
        protected ConstraintAttributeBase()
        {
        }

        /// <summary>
        /// Protected constructor.
        /// </summary>
        /// <param name="value"></param>
        protected ConstraintAttributeBase(T value)
            : base(value)
        {
        }
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
        /// <summary>
        /// Internal default constructor.
        /// </summary>
        /// <remarks>The constructor is internal for a reason, because do not want to over
        /// expose beyond the fluent interface more than is absolutely necessary.</remarks>
        internal ClusteredConstraintAttribute()
        {
        }
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
        /// <summary>
        /// Internal default constructor.
        /// </summary>
        /// <remarks>The constructor is internal for a reason, because do not want to over
        /// expose beyond the fluent interface more than is absolutely necessary.</remarks>
        internal TableIndexConstraintAttribute()
        {
        }
    }

    /// <summary>
    /// With Values constraint attribute.
    /// </summary>
    public interface IWithValuesConstraintAttribute : IConstraintAttribute
    {
    }

    /// <summary>
    /// With Values constraint attribute.
    /// </summary>
    /// <see cref="!:https://msdn.microsoft.com/en-us/library/ms187742.aspx"
    /// >column_definition (Transact-SQL)</see>
    public class WithValuesConstraintAttribute : ConstraintAttributeBase
    {
        internal static readonly WithValuesConstraintAttribute Instance
            = new WithValuesConstraintAttribute();

        /// <summary>
        /// Internal default constructor.
        /// </summary>
        /// <remarks>This is internal for a reason, because it does not need to be exposed
        /// to the outer edges.</remarks>
        internal WithValuesConstraintAttribute()
        {
        }
    }

    /// <summary>
    /// Not for replication constraint attribute.
    /// </summary>
    public interface INotForReplicationConstraintAttribute : IConstraintAttribute
    {
    }

    /// <summary>
    /// Not for replication constraint attribute.
    /// </summary>
    public class NotForReplicationConstraintAttribute
        : ConstraintAttributeBase
            , INotForReplicationConstraintAttribute
    {
        internal static readonly NotForReplicationConstraintAttribute Instance
            = new NotForReplicationConstraintAttribute();
    }

    /// <summary>
    /// Foreign key action constraint attribute.
    /// </summary>
    public interface IForeignKeyActionConstraintAttribute : IConstraintAttribute<ForeignKeyTrigger>
    {

        /// <summary>
        /// Gets or sets the Action.
        /// </summary>
        ForeignKeyAction Action { get; set; }
    }

    /// <summary>
    /// Foreign key action constraint attribute.
    /// </summary>
    public class ForeignKeyActionConstraintAttribute
        : ConstraintAttributeBase<ForeignKeyTrigger>
            , IForeignKeyActionConstraintAttribute
    {
        /// <summary>
        /// Gets or sets the Action.
        /// </summary>
        public ForeignKeyAction Action { get; set; }

        /// <summary>
        /// Internal default constructor.
        /// </summary>
        /// <remarks>This is internal for a reason, because it does not need to be exposed
        /// to the outer edges.</remarks>
        internal ForeignKeyActionConstraintAttribute()
        {
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="action"></param>
        /// <remarks>This is internal for a reason, because it does not need to be exposed
        /// to the outer edges.</remarks>
        internal ForeignKeyActionConstraintAttribute(ForeignKeyAction action)
        {
            Action = action;
        }
    }

    /// <summary>
    /// If Exists constraint attribute.
    /// </summary>
    public interface IIfExistsConstraintAttribute : IConstraintAttribute
    {
    }

    /// <summary>
    /// If Exists constraint attribute.
    /// </summary>
    public class IfExistsConstraintAttribute : ConstraintAttributeBase, IIfExistsConstraintAttribute
    {
        /// <summary>
        /// Private constructor.
        /// </summary>
        /// <remarks>This is private for a reason, because it does not need to be exposed
        /// to the outer edges.</remarks>
        private IfExistsConstraintAttribute()
        {
        }

        internal static readonly IIfExistsConstraintAttribute DefaultInstance = new IfExistsConstraintAttribute();
    }
}
