namespace Kingdom.Data
{
    /// <summary>
    /// Represents general Column Attributes.
    /// </summary>
    public interface IColumnAttribute : IDataAttribute
    {
    }

    /// <summary>
    /// Represents a column attribute.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IColumnAttribute<T> : IDataAttribute<T>, IColumnAttribute
    {
    }

    /// <summary>
    /// Precision column attribute.
    /// </summary>
    public interface IPrecisionColumnAttribute : IColumnAttribute<int>
    {
    }

    /// <summary>
    /// Scale column attribute.
    /// </summary>
    public interface IScaleColumnAttribute : IColumnAttribute<int>
    {
    }

    /// <summary>
    /// Nullable column attribute.
    /// </summary>
    public interface INullableColumnAttribute : IColumnAttribute<bool>
    {
    }

    /// <summary>
    /// Sort Order column attribute.
    /// </summary>
    /// <see cref="SortOrder"/>
    public interface ISortOrderColumnAttribute : IColumnAttribute<SortOrder>
    {
    }

    /// <summary>
    /// If Exists column attribute.
    /// </summary>
    public interface IIfExistsColumnAttribute : IColumnAttribute
    {
    }

    /// <summary>
    /// Represents a basic valueless Column attribute.
    /// </summary>
    public class ColumnAttributeBase : DataAttributeBase, IColumnAttribute
    {
    }

    /// <summary>
    /// Column attribute base class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ColumnAttributeBase<T> : DataAttributeBase<T>, IColumnAttribute<T>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        protected ColumnAttributeBase()
        {
        }

        /// <summary>
        /// Protected constructor.
        /// </summary>
        /// <param name="value"></param>
        protected ColumnAttributeBase(T value)
            : base(value)
        {
        }
    }

    /// <summary>
    /// Precision column attribute. Used for length for character based and similar types.
    /// Also used for numeric for numerical precision.
    /// </summary>
    public class PrecisionColumnAttribute : ColumnAttributeBase<int>, IPrecisionColumnAttribute
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public PrecisionColumnAttribute()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value"></param>
        public PrecisionColumnAttribute(int value)
            : base(value)
        {
        }
    }

    /// <summary>
    /// Scale column attribute.
    /// </summary>
    public class ScaleColumnAttribute : ColumnAttributeBase<int>, IScaleColumnAttribute
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScaleColumnAttribute()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value"></param>
        public ScaleColumnAttribute(int value)
            : base(value)
        {
        }
    }

    /// <summary>
    /// Nullable column attribute.
    /// </summary>
    public class NullableColumnAttribute : ColumnAttributeBase<bool>, INullableColumnAttribute
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public NullableColumnAttribute()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="state"></param>
        public NullableColumnAttribute(bool state)
            : base(state)
        {
        }
    }

    /// <summary>
    /// Represents the basic Identity column atrribute.
    /// </summary>
    public interface IIdentityColumnAttribute : IColumnAttribute
    {
    }

    /// <summary>
    /// Represents the basic Identity column attribute.
    /// </summary>
    public class IdentityColumnAttribute : ColumnAttributeBase, IIdentityColumnAttribute
    {
    }

    /// <summary>
    /// Represents a seeded Identity column atrribute.
    /// </summary>
    public interface ISeededIdentityColumnAttribute : IColumnAttribute<int>
    {
        /// <summary>
        /// Gets or sets the Increment.
        /// </summary>
        int Increment { get; set; }
    }

    /// <summary>
    /// Represents a seeded Identity column attribute.
    /// </summary>
    public class SeededIdentityColumnAttribute : ColumnAttributeBase<int>, ISeededIdentityColumnAttribute
    {
        public int Increment { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SeededIdentityColumnAttribute()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="increment"></param>
        public SeededIdentityColumnAttribute(int seed, int increment)
            : base(seed)
        {
            Increment = increment;
        }
    }

    /// <summary>
    /// Sort Order column attribute.
    /// </summary>
    public class SortOrderColumnAttribute : ColumnAttributeBase<SortOrder>, ISortOrderColumnAttribute
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SortOrderColumnAttribute()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value"></param>
        public SortOrderColumnAttribute(SortOrder value)
            : base(value)
        {
        }
    }

    /// <summary>
    /// If Exists column attribute.
    /// </summary>
    public class IfExistsColumnAttribute : ColumnAttributeBase, IIfExistsColumnAttribute
    {
        /// <summary>
        /// Private constructor.
        /// </summary>
        /// <remarks>This is private for a reason, because it does not need to be exposed
        /// to the outer edges.</remarks>
        private IfExistsColumnAttribute()
        {
        }

        internal static IIfExistsColumnAttribute DefaultInstance = new IfExistsColumnAttribute();
    }
}
