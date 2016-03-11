using System.Collections.Generic;

namespace Kingdom.Data
{
    /// <summary>
    /// Represents a fluent root base class.
    /// </summary>
    public abstract class FluentRootBase
    {
        /// <summary>
        /// Returns a comma delimited set of <paramref name="clauses"/>.
        /// </summary>
        /// <param name="clauses"></param>
        /// <returns></returns>
        protected string CommaDelimited(IEnumerable<object> clauses)
        {
            return string.Join(", ", clauses);
        }
    }
}
