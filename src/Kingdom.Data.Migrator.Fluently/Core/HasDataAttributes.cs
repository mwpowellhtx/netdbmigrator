using System.Collections.Generic;

namespace Kingdom.Data
{
    /// <summary>
    /// Indicates that the interface has data attributes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <see cref="IDataAttribute"/>
    public interface IHasDataAttributes<T>
        where T : IDataAttribute
    {
        /// <summary>
        /// Gets or sets the Attributes.
        /// </summary>
        IList<T> Attributes { get; set; }
    }
}
