namespace Kingdom.Data
{
    /// <summary>
    /// Represents general Column Attributes.
    /// </summary>
    public interface IColumnAttribute
    {
    }

    /// <summary>
    /// Precision column attribute.
    /// </summary>
    public interface IPrecisionColumnAttribute : IColumnAttribute
    {
        /// <summary>
        /// Gets or sets the Precision.
        /// </summary>
        int Precision { get; set; }
    }

    /// <summary>
    /// Scale column attribute.
    /// </summary>
    public interface IScaleColumnAttribute : IColumnAttribute
    {
        /// <summary>
        /// Gets or sets the Scale.
        /// </summary>
        int Scale { get; set; }
    }

    /// <summary>
    /// Nullable column attribute.
    /// </summary>
    public interface INullableColumnAttribute : IColumnAttribute
    {
        /// <summary>
        /// Gets or sets whether CanBeNullable.
        /// </summary>
        bool CanBeNull { get; set; }
    }

    /// <summary>
    /// Column attribute base class.
    /// </summary>
    public abstract class ColumnAttributeBase : IColumnAttribute
    {
    }

    /// <summary>
    /// Precision column attribute. Used for length for character based and similar types.
    /// Also used for numeric for numerical precision.
    /// </summary>
    public class PrecisionColumnAttribute : ColumnAttributeBase, IPrecisionColumnAttribute
    {
        public int Precision { get; set; }
    }

    /// <summary>
    /// Scale column attribute.
    /// </summary>
    public class ScaleColumnAttribute : ColumnAttributeBase, IScaleColumnAttribute
    {
        public int Scale { get; set; }
    }

    /// <summary>
    /// Nullable column attribute.
    /// </summary>
    public class NullableColumnAttribute : ColumnAttributeBase, INullableColumnAttribute
    {
        public bool CanBeNull { get; set; }
    }
}
