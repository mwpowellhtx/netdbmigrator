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
    /// <typeparam name="T"></typeparam>
    public abstract class DataAttributeBase<T> : IDataAttribute<T>
    {
        public T Value { get; set; }
    }
}
