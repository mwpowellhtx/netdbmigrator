using System;
using System.Collections.Generic;
using System.Linq;

namespace Kingdom.Data
{
    /// <summary>
    /// Provides basic data functionality across concerns.
    /// </summary>
    public abstract class DataBase
    {
        /// <summary>
        /// Tries to return the value of the attributed column through <paramref name="result"/>.
        /// If the attribute cannot be found, returns false.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="attributes"></param>
        /// <param name="result"></param>
        /// <param name="getter"></param>
        /// <returns></returns>
        protected static bool TryFindColumnAttribute<TAttribute, TResult>(
            IEnumerable<IDataAttribute> attributes, out TResult result
            , Func<TAttribute, TResult> getter)
            where TAttribute : IDataAttribute
        {
            var attribute = attributes.OfType<TAttribute>().SingleOrDefault();
            var found = attribute != null;
            result = found ? getter(attribute) : default(TResult);
            return found;
        }

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
    public abstract class DataBase<TDataAttribute> : DataBase
        where TDataAttribute : IDataAttribute
    {
        private IList<TDataAttribute> _attributes;

        /// <summary>
        /// Gets or sets the Attributes.
        /// </summary>
        public IList<TDataAttribute> Attributes
        {
            get { return _attributes; }
            set { _attributes = value ?? new List<TDataAttribute>(); }
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
