namespace Kingdom.Data
{
    /// <summary>
    /// Indicates that the interface has data attributes.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <see cref="IDataAttribute"/>
    public interface IHasDataAttributes<T, out TParent>
        where T : IDataAttribute
        where TParent : IHasDataAttributes<T, TParent>
    {
        /// <summary>
        /// Gets the Attributes.
        /// </summary>
        IFluentCollection<T, TParent> Attributes { get; }
    }
}
