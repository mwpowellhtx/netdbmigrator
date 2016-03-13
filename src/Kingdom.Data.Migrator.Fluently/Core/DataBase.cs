using System.Collections.Generic;

namespace Kingdom.Data
{
    /// <summary>
    /// Provides basic data functionality across concerns.
    /// </summary>
    public abstract class DataBase
    {
        /// <summary>
        /// Returns a comma delimited set of <paramref name="clauses"/>.
        /// </summary>
        /// <param name="clauses"></param>
        /// <returns></returns>
        protected static string CommaDelimited(IEnumerable<object> clauses)
        {
            return string.Join(", ", clauses);
        }
    }
    
    /// <summary>
    /// Basic database operational support.
    /// </summary>
    /// <typeparam name="TDataAttribute"></typeparam>
    /// <typeparam name="TParent"></typeparam>
    public abstract class DataBase<TDataAttribute, TParent> : DataBase
        where TDataAttribute : IDataAttribute
        where TParent : DataBase<TDataAttribute, TParent>
    {
        private IFluentCollection<TDataAttribute, TParent> _attributes;

        /// <summary>
        /// Gets or sets the Attributes.
        /// </summary>
        public IFluentCollection<TDataAttribute, TParent> Attributes
        {
            get { return _attributes; }
            private set
            {
                _attributes
                    = value
                      ?? new FluentCollection<TDataAttribute, TParent>((TParent) this);
            }
        }

        /// <summary>
        /// Protected constructor.
        /// </summary>
        protected DataBase()
        {
            Attributes = null;
        }
    }
}
