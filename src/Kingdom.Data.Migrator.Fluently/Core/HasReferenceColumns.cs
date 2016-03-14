namespace Kingdom.Data
{
    /// <summary>
    /// Represents a concept that has reference columns.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TParent"></typeparam>
    public interface IHasColumns<T, out TParent>
        where T : IColumn
    {
        /// <summary>
        /// Gets the Reference Columns.
        /// </summary>
        IFluentCollection<T, TParent> Columns { get; } 
    }
}
