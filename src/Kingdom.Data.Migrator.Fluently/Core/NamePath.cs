using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kingdom.Data
{
    /// <summary>
    /// Named path interface.
    /// </summary>
    public interface INamePath : IList<string>
    {
        /// <summary>
        /// Gets or sets whether to IgnoreEmptyNodes.
        /// </summary>
        bool IgnoreEmptyNodes { get; set; }

        /// <summary>
        /// Gets or sets the path Delimiter.
        /// </summary>
        string Delimiter { get; set; }

        /// <summary>
        /// Gets or sets the NodeDecorator function.
        /// </summary>
        string NodeDecorator { get; set; }

        /// <summary>
        /// Returns the string representation of the name path.
        /// </summary>
        /// <returns></returns>
        string ToString();
    }

    /// <summary>
    /// Represents a name path complete with formatting and delimiters.
    /// </summary>
    public class NamePath : INamePath
    {
        /// <summary>
        /// Gets or sets whether to IgnoreEmptyNodes.
        /// </summary>
        public bool IgnoreEmptyNodes { get; set; }

        /// <summary>
        /// Default delimiter: &quot;, &quot;
        /// </summary>
        public const string DefaultDelimiter = ".";

        private string _delimiter;

        /// <summary>
        /// Gets or sets the Delimiter. Default value is <see cref="DefaultDelimiter"/>.
        /// </summary>
        public string Delimiter
        {
            get { return _delimiter; }
            set { _delimiter = value ?? DefaultDelimiter; }
        }

        /// <summary>
        /// Default node decorator.
        /// </summary>
        /// <value>&quot;[]&quot;</value>
        public const string DefaultNodeDecorator = @"[]";

        private string _decorator;

        public string NodeDecorator
        {
            get { return _decorator; }
            set { _decorator = value ?? DefaultNodeDecorator; }
        }

        private readonly IList<string> _nodes;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NamePath()
            : this(new string[0])
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nodes"></param>
        public NamePath(IEnumerable<string> nodes)
        {
            _nodes = nodes.ToList();
            IgnoreEmptyNodes = true;
            Delimiter = DefaultDelimiter;
            NodeDecorator = DefaultNodeDecorator;
        }

        private void NodeAction(Action<IList<string>> action)
        {
            action(_nodes);
        }

        private T NodeFunc<T>(Func<IList<string>, T> func)
        {
            return func(_nodes);
        }

        public int IndexOf(string item)
        {
            return NodeFunc(x => x.IndexOf(item));
        }

        public void Insert(int index, string item)
        {
            NodeAction(x => x.Insert(index, item.Trim()));
        }

        public void RemoveAt(int index)
        {
            NodeAction(x => x.RemoveAt(index));
        }

        public string this[int index]
        {
            get { return NodeFunc(x => x[index]); }
            set { NodeAction(x => x[index] = (value ?? string.Empty).Trim()); }
        }

        public void Add(string item)
        {
            NodeAction(x => x.Add(item));
        }

        public void Clear()
        {
            NodeAction(x => x.Clear());
        }

        public bool Contains(string item)
        {
            return NodeFunc(x => x.Contains(item));
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            NodeAction(x => x.CopyTo(array, arrayIndex));
        }

        public int Count
        {
            get { return NodeFunc(x => x.Count); }
        }

        public bool IsReadOnly
        {
            get { return NodeFunc(x => x.IsReadOnly); }
        }

        public bool Remove(string item)
        {
            return NodeFunc(x => x.Remove(item));
        }

        public IEnumerator<string> GetEnumerator()
        {
            return NodeFunc(x => x.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private string GetNodeDecorator(int index)
        {
            return NodeDecorator.Length != 2
                ? string.Empty
                : string.Format("{0}", NodeDecorator[index]);
        }

        /// <summary>
        /// Creates a new <see cref="INamePath"/> based on <paramref name="node"/> and
        /// <paramref name="nodes"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static INamePath Create(string node, params string[] nodes)
        {
            var local = nodes.ToList();
            local.Insert(0, node);
            return new NamePath(local);
        }

        public override string ToString()
        {
            var nodes = _nodes.Where(x => !IgnoreEmptyNodes || !string.IsNullOrEmpty(x));
            var decorated = nodes.Select(x => string.Format("{0}{1}{2}", GetNodeDecorator(0), x, GetNodeDecorator(1)));
            return decorated.Aggregate(string.Empty, (g, x) => string.IsNullOrEmpty(g) ? x : g + Delimiter + x);
        }
    }
}
