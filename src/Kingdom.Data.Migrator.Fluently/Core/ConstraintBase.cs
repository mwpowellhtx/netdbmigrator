using System.Collections.Generic;

namespace Kingdom.Data
{
    /// <summary>
    /// Represents the base Constraint concept.
    /// </summary>
    public interface IConstraint
        : ITableAddable
            , ITableDroppable
    {
        /// <summary>
        /// Gets or sets the Constraint Name.
        /// </summary>
        INamePath Name { get; set; }
    }

    /// <summary>
    /// Represents the base Constraint concept.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public abstract class ConstraintBase<TParent>
        : DataBase<IConstraintAttribute, TParent>
            , IConstraint
        where TParent : ConstraintBase<TParent>
    {
        /// <summary>
        /// Gets the SubjectName: &quot;CONSTRAINT&quot;
        /// </summary>
        public string SubjectName
        {
            get { return "CONSTRAINT"; }
        }

        /// <summary>
        /// Gets or sets the Constraint Name.
        /// </summary>
        public INamePath Name { get; set; }

        /// <summary>
        /// Gets or sets the ColumnNames.
        /// </summary>
        public IList<INamePath> ColumnNames { get; set; }

        /// <summary>
        /// Returns the string representing the Fluent addable operation.
        /// </summary>
        /// <returns></returns>
        public abstract string GetAddableString();

        /// <summary>
        /// Returns the string supporting the Fluent droppable operation.
        /// </summary>
        /// <returns></returns>
        public virtual string GetDroppableString()
        {
            return Name.ToString();
        }
    }

    /// <summary>
    /// Indicates that the interface should have index columns.
    /// </summary>
    /// <typeparam name="TColumn"></typeparam>
    /// <typeparam name="TParent"></typeparam>
    public interface IHasIndexColumns<TColumn, TParent>
        where TColumn : IColumn
        where TParent : IHasIndexColumns<TColumn, TParent>
    {
        /// <summary>
        /// Represents the key Columns.
        /// </summary>
        IFluentCollection<TColumn, TParent> KeyColumns { get; set; }
    }

    // TODO: fill in the column gaps next
    public interface IForeignKeyConstraint<TParent>
        : IConstraint
            , IHasIndexColumns<IColumn, TParent>
        where TParent : IForeignKeyConstraint<TParent>
    {
        INamePath ReferenceTableName { get; set; }

        IList<INamePath> ReferenceColumns { get; set; }

        ForeignKeyAction? OnDeleteAction { get; set; }

        ForeignKeyAction? OnUpdateAction { get; set; }
    }
}
