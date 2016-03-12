namespace Kingdom.Data
{
    /// <summary>
    /// Represents a fluent type.
    /// </summary>
    public interface IFluently
    {
    }

    /// <summary>
    /// Represents an Alter fluent type.
    /// </summary>
    /// <typeparam name="TAlterTableFluently"></typeparam>
    public interface IAlterFluently<out TAlterTableFluently> : IFluently
        where TAlterTableFluently : class, IAlterTableFluently, new()
    {
        /// <summary>
        /// Initiates an Alter Table statement being built.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="withCheck"></param>
        /// <returns></returns>
        TAlterTableFluently Table(INamePath name, CheckType? withCheck = null);
    }

    /// <summary>
    /// Represents an Alter Table type capable of adding to the definition.
    /// </summary>
    public interface IAlterTableAddFluently : IFluently
    {
        /// <summary>
        /// Adds zero or more <paramref name="items"/> to the definition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        IAlterTableAddFluently Add<T>(params T[] items)
            where T : class, ITableAddable;

        /// <summary>
        /// Returns a string representation for this fluent operation.
        /// </summary>
        /// <returns></returns>
        string ToString();
    }

    /// <summary>
    /// Represents an Alter Table type capable of dropping from the definition.
    /// </summary>
    public interface IAlterTableDropFluently : IFluently
    {
        /// <summary>
        /// Drops zero or more <paramref name="items"/> from the definition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        IAlterTableDropFluently Drop<T>(params T[] items)
            where T : class, ITableDroppable;
    }

    /// <summary>
    /// Represents fluently being able to Alter Table.
    /// </summary>
    public interface IAlterTableFluently
        : IAlterTableAddFluently
            , IAlterTableDropFluently
    {
        /// <summary>
        /// Gets or sets the TableName.
        /// </summary>
        INamePath TableName { get; set; }

        /// <summary>
        /// Gets or sets WithCheck.
        /// </summary>
        CheckType? WithCheck { get; set; }
    }

    //TODO: TBD: may need to consider an alter table fluent "agent" of sorts; sort of an ALTER TABLE <AGENT/>
}
