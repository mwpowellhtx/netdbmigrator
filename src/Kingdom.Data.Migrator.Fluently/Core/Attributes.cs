namespace Kingdom.Data
{
    /// <summary>
    /// Data attribute interface.
    /// </summary>
    public interface IDataAttribute
    {
    }

    /// <summary>
    /// Data attribute interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataAttribute<T> : IDataAttribute
    {
        /// <summary>
        /// Gets or sets the attribute Value.
        /// </summary>
        T Value { get; set; }
    }

    /// <summary>
    /// Data attribute concept.
    /// </summary>
    public abstract class DataAttributeBase : IDataAttribute
    {
    }

    /// <summary>
    /// Data attribute concept.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DataAttributeBase<T> : DataAttributeBase, IDataAttribute<T>
    {
        public T Value { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected DataAttributeBase()
        {
        }

        /// <summary>
        /// Protected constructor.
        /// </summary>
        /// <param name="value"></param>
        protected DataAttributeBase(T value)
        {
            Value = value;
        }
    }
}
