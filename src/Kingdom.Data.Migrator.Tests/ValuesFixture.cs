using System;
using System.Collections.Generic;
using System.Linq;

namespace Kingdom.Data
{
    public interface IValuesFixture<T>
    {
        IList<T> Values { get; } 
    }

    /// <summary>
    /// Gathers some related values into a list that is friendly towards unit test runner display.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValuesFixture<T> : IValuesFixture<T>
    {
        private readonly Func<T, string> _stringify;

        private readonly IList<T> _values;

        public IList<T> Values
        {
            get { return _values; }
        }

        internal ValuesFixture(Func<T, string> stringify, params T[] values)
        {
            _stringify = stringify;
            _values = values.ToList();
        }

        internal ValuesFixture(params T[] values)
            : this(x => x.ToString(), values)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}[]{{{1}}}", typeof (T).FullName,
                string.Join(", ", _values.Select(_stringify)));
        }
    }

    internal static class ValuesFixtureExtensionMethods
    {
        public static IValuesFixture<T> ToValuesFixture<T>(this IEnumerable<T> values)
        {
            return new ValuesFixture<T>(values.ToArray());
        }
    }
}
