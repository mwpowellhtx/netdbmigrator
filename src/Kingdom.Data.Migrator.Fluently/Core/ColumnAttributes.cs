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
    /// Column attribute base class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ColumnAttributeBase<T> : DataAttributeBase<T>, IColumnAttribute<T>
    {
    }

    /// <summary>
    /// Precision column attribute. Used for length for character based and similar types.
    /// Also used for numeric for numerical precision.
    /// </summary>
    public class PrecisionColumnAttribute : ColumnAttributeBase<int>, IPrecisionColumnAttribute
    {
    }

    /// <summary>
    /// Scale column attribute.
    /// </summary>
    public class ScaleColumnAttribute : ColumnAttributeBase<int>, IScaleColumnAttribute
    {
    }

    /// <summary>
    /// Nullable column attribute.
    /// </summary>
    public class NullableColumnAttribute : ColumnAttributeBase<bool>, INullableColumnAttribute
    {
    }

    /// <summary>
    /// Sort Order column attribute.
    /// </summary>
    public class SortOrderColumnAttribute : ColumnAttributeBase<SortOrder>, ISortOrderColumnAttribute
    {
    }
}
