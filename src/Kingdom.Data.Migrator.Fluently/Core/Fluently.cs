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
        where TAlterTableFluently : class, IAlterTableFluently<TAlterTableFluently>, new()
    {
        /// <summary>
        /// Initiates an Alter Table statement being built.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        TAlterTableFluently Table(INamePath name);
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
    /// <typeparam name="TParent"></typeparam>
    public interface IAlterTableFluently<out TParent>
        : IAlterTableAddFluently
            , IAlterTableDropFluently
        where TParent : IAlterTableFluently<TParent>
    {
        /// <summary>
        /// Gets or sets the TableName.
        /// </summary>
        INamePath TableName { get; set; }

        /// <summary>
        /// Sets the <paramref name="checkType"/> for the fluent Alter Table sentence.
        /// </summary>
        /// <param name="checkType"></param>
        /// <returns></returns>
        TParent With(CheckType? checkType = null);
    }

    //TODO: TBD: may need to consider an alter table fluent "agent" of sorts; sort of an ALTER TABLE <AGENT/>
}
