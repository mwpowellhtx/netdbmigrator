using System;
using System.Collections.Generic;
using System.Linq;

namespace Kingdom.Data
{
    /// <summary>
    /// Fluent collection interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TParent"></typeparam>
    public interface IFluentCollection<T, out TParent>
    {
        /// <summary>
        /// Adds items to the collection and allows the parent to continue fluently chaining.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        TParent Add(T item, params T[] items);
    }

    /// <summary>
    /// Fluent collection class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TParent"></typeparam>
    public class FluentCollection<T, TParent> : IFluentCollection<T, TParent>
    {
        private readonly TParent _parent;

        private readonly IList<T> _items;

        internal FluentCollection(TParent parent)
        {
            _parent = parent;
            _items = new List<T>();
        }

        internal IList<T> Items
        {
            get { return _items; }
        }

        public TParent Add(T item, params T[] items)
        {
            _items.Add(item);
            foreach (var x in items)
                _items.Add(x);
            return _parent;
        }
    }

    internal static class FluentCollectionExtensionMethods
    {
        /// <summary>
        /// Returns the string representation of the <paramref name="collection"/>
        /// based on each item.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="formatter"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToString<T, TParent>(
            this IFluentCollection<T, TParent> collection, Func<T, string> formatter)
        {
            return ((FluentCollection<T, TParent>) collection).Items.Select(formatter);
        }
    }
}
