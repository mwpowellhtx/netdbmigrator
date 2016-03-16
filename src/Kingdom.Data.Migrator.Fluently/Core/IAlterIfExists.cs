namespace Kingdom.Data
{
    /// <summary>
    /// Signals to render If Exists clause in the statement.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public interface IAlterIfExists<out TParent>
        where TParent : IAlterIfExists<TParent>
    {
        /// <summary>
        /// Signals to render If Exists.
        /// </summary>
        /// <returns></returns>
        TParent IfExists();
    }
}
