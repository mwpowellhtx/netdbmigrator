namespace Kingdom.Data
{
    /// <summary>
    /// Table Addable interface.
    /// </summary>
    public interface ITableAddable : ISubject
    {
        /// <summary>
        /// Returns the Addable string.
        /// </summary>
        /// <returns></returns>
        string GetAddableString();
    }

    /// <summary>
    /// Table Droppable interface.
    /// </summary>
    public interface ITableDroppable : ISubject
    {
        /// <summary>
        /// Returns the Droppable string.
        /// </summary>
        /// <returns></returns>
        string GetDroppableString();
    }

    /// <summary>
    /// Creatable interface.
    /// </summary>
    public interface ICreatable
    {
        /// <summary>
        /// Returns the Creatable string.
        /// </summary>
        /// <returns></returns>
        string GetCreateString();
    }
}
