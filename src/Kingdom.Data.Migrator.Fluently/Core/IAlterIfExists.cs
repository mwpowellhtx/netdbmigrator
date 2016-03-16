namespace Kingdom.Data
{
    /// <summary>
    /// Signals to render If Exists clause in the statement.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    /// <see cref="!:http://msdn.microsoft.com/en-us/library/ms190273.aspx" >ALTER TABLE (Transact-SQL), Sql Server 2014+</see>
    /// <remarks>Does not apply for versions through Sql Server 2012.</remarks>
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
