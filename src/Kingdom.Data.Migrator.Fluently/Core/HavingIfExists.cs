namespace Kingdom.Data
{
    /// <summary>
    /// Represents whether Having If Exists.
    /// </summary>
    /// <see cref="!:http://msdn.microsoft.com/en-us/library/ms190273.aspx" >ALTER TABLE (Transact-SQL), Sql Server 2014+</see>
    /// <remarks>Does not apply for versions through Sql Server 2012.</remarks>
    public interface IHavingIfExists
    {
        /// <summary>
        /// Gets whether Has If Exists.
        /// </summary>
        bool HasIfExists { get; }
    }
}
