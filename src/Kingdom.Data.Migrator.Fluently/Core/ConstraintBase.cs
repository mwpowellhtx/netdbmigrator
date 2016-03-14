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
        /// Provides a way to set a valueless attribute in the constraint.
        /// <paramref name="factory"/> is a concession parameter that permits a new attribute
        /// to be created without requiring the new generic constraint, and thereby exposing
        /// the attribute outside the assembly. But which also has the side benefit of shortening
        /// the code to a more concise version when calling.
        /// </summary>
        /// <typeparam name="TConstraintAttribute"></typeparam>
        /// <param name="factory"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        protected TParent Set<TConstraintAttribute>(Func<TConstraintAttribute> factory,
            Func<TConstraintAttribute, bool> predicate = null)
            where TConstraintAttribute : class, IConstraintAttribute
        {
            TConstraintAttribute attribute;
            if (!Attributes.TryFindColumnAttribute(out attribute, x => x, predicate))
                Attributes.Add(factory());
            return GetThisParent();
        }

        /// <summary>
        /// Provides a way to set attribute values in the constraint. <paramref name="factory"/>
        /// is a concession parameter that permits a new attribute to be created without requiring
        /// the new generic constraint, and thereby exposing the attribute outside the assembly.
        /// But which also has the side benefit of shortening the code to a more concise version
        /// when calling.
        /// </summary>
        /// <typeparam name="TConstraintAttribute"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="factory"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        protected TParent Set<TConstraintAttribute, TValue>(TValue value,
            Func<TConstraintAttribute> factory, Func<TConstraintAttribute, bool> predicate = null)
            where TConstraintAttribute : class, IConstraintAttribute<TValue>
        {
            TConstraintAttribute attribute;
            if (!Attributes.TryFindColumnAttribute(out attribute, x => x, predicate))
                Attributes.Add(attribute = factory());
            attribute.Value = value;
            return GetThisParent();
        }

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
}