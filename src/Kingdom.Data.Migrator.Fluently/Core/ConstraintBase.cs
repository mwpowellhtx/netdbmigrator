using System;
using System.Collections.Generic;

namespace Kingdom.Data
{
    /// <summary>
    /// Represents the base Constraint concept.
    /// </summary>
    public interface IConstraint
        : ITableAddable
            , ITableDroppable
            , IFluentCollection<IConstraintAttribute, IConstraint>
    {
        /// <summary>
        /// Gets or sets the Constraint Name.
        /// </summary>
        INamePath Name { get; set; }
    }

    /// <summary>
    /// Represents the base Constraint concept.
    /// </summary>
    public abstract class ConstraintBase
        : DataBase<IConstraintAttribute>
            , IConstraint
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

        public IConstraint Add(IConstraintAttribute item, params IConstraintAttribute[] items)
        {
            Attributes.Add(item);
            foreach (var x in items)
                Attributes.Add(x);
            return this;
        }

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
    public interface IHasIndexColumns<TParent>
    {
        /// <summary>
        /// Represents the key Columns.
        /// </summary>
        IFluentCollection<IColumn, TParent> KeyColumns { get; set; }
    }

    public interface IForeignKeyConstraint<TParent> : IConstraint, IHasIndexColumns<TParent>
    {
        INamePath ReferenceTableName { get; set; }

        IList<INamePath> ReferenceColumns { get; set; }

        ForeignKeyAction? OnDeleteAction { get; set; }

        ForeignKeyAction? OnUpdateAction { get; set; }
    }

    public interface IDefaultConstraint : IConstraint
    {
        IColumn Column { get; set; }
    }

    public interface IDefaultConstraint<T> : IDefaultConstraint
    {
        T Value { get; set; }

        Func<T, string> ValueFormatter { get; set; }
    }
}
