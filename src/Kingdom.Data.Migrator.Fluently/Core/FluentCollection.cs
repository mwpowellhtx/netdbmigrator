using System;
using System.Collections.Generic;
using System.Linq;

namespace Kingdom.Data
{
    /// <summary>
    /// Fluent collection items interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFluentCollectionItems<T>
    {
        /// <summary>
        /// Gets the Items.
        /// </summary>
        IList<T> Items { get; }
    }

    /// <summary>
    /// Fluent collection interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TParent"></typeparam>
    public interface IFluentCollection<T, out TParent> : IFluentCollectionItems<T>
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

        /// <summary>
        /// Gets the Items.
        /// </summary>
        public IList<T> Items
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
        /// Tries to return the value of the attributed column through <paramref name="result"/>.
        /// If the attribute cannot be found, returns false.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="attributes"></param>
        /// <param name="result"></param>
        /// <param name="getter"></param>
        /// <returns></returns>
        public static bool TryFindColumnAttribute<TAttribute, TResult>(
            this IEnumerable<IDataAttribute> attributes,
            out TResult result, Func<TAttribute, TResult> getter)
            where TAttribute : IDataAttribute
        {
            var attribute = attributes.OfType<TAttribute>().SingleOrDefault();
            var found = attribute != null;
            result = found ? getter(attribute) : default(TResult);
            return found;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="TAttribute"></typeparam>
        ///// <typeparam name="TParent"></typeparam>
        ///// <typeparam name="TResult"></typeparam>
        ///// <param name="collection"></param>
        ///// <param name="result"></param>
        ///// <param name="getter"></param>
        ///// <returns></returns>
        //public static bool TryFindColumnAttribute<TAttribute, TParent, TResult>(
        //    this IFluentCollection<IDataAttribute, TParent> collection,
        //    out TResult result, Func<TAttribute, TResult> getter)
        //    where TAttribute : IDataAttribute
        //{
        //    var attribute = collection.Items.OfType<TAttribute>().SingleOrDefault();
        //    var found = attribute != null;
        //    result = found ? getter(attribute) : default(TResult);
        //    return found;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TItemBase"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="collection"></param>
        /// <param name="result"></param>
        /// <param name="getter"></param>
        /// <returns></returns>
        public static bool TryFindColumnAttribute<TItem, TItemBase, TResult>(
            this IFluentCollectionItems<TItemBase> collection,
            out TResult result, Func<TItem, TResult> getter)
            where TItemBase : IDataAttribute
            where TItem : TItemBase
        {
            var attribute = collection.Items.OfType<TItem>().SingleOrDefault();
            var found = attribute != null;
            result = found ? getter(attribute) : default(TResult);
            return found;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TItemBase"></typeparam>
        /// <param name="collection"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool TryColumnAttributeExists<TItem, TItemBase>(
            this IFluentCollectionItems<TItemBase> collection,
            Func<TItem, bool> match = null)
        {
            match = match ?? (x => true);
            return collection.Items.OfType<TItem>().Any(match);
        }
    }
}
