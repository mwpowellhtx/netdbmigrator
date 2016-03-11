using System;
using System.Data;
using System.Linq;

namespace Kingdom.Data
{
    /// <summary>
    /// Data type registry represents mapping <see cref="DbType"/> ubiquitously across database
    /// providers.
    /// </summary>
    public interface IDataTypeRegistry
    {
        /// <summary>
        /// Returns the string representation of the <paramref name="type"/> and
        /// <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        string GetDbTypeString(DbType type, int? a = null, int? b = null);
    }

    /// <summary>
    /// Data type registry represents mapping <see cref="DbType"/> ubiquitously across database
    /// providers.
    /// </summary>
    public abstract class DataTypeRegistryBase : IDataTypeRegistry
    {
        /// <summary>
        /// Returns the formatted data type <paramref name="a"/> and <paramref name="b"/>. In most
        /// cases, <paramref name="a"/> will represent the length. In a smaller set of use caess,
        /// <paramref name="a"/> represents precision, whereas, <paramref name="b"/> represents
        /// scale. We will prefer the former first for precision or length, followed by the latter
        /// optional part. When the first is null and the second is specified, we ignore the first
        /// and assume the second was meant as the first.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected static string GetDataTypeLength(int? a = null, int? b = null)
        {
            const int @default = default(int);

            var parts = new[] {a, b}.Where(x => x != null).ToArray();

            Func<int, int, string> format2 = (x, y) => string.Format(@"({0}, {1})", x, y);
            Func<int, string> format = x => string.Format(@"({0})", x == int.MaxValue ? "MAX" : x.ToString());

            var count = parts.Length;

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch(count)
            {
                case 2:

                    var first = parts.First() ?? @default;
                    var last = parts.Last() ?? @default;
                    return format2(first, last);

                case 1:

                    var part = parts.Single(x => x != null) ?? @default;
                    return format(part);
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns an <see cref="Exception"/> reflecting that <paramref name="type"/> is not
        /// supported.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected static Exception ThrowNotSupportedException<TDbType>(TDbType type, int? a, int? b)
        {
            // TODO: may enforce that Type should be an Enum type.
            var message = string.Format("Database type {0}{1} is unsupported.", type,
                GetDataTypeLength(a, b));
            return new NotSupportedException(message);
        }

        /// <summary>
        /// Returns the string representation of the <paramref name="type"/> and
        /// <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public virtual string GetDbTypeString(DbType type, int? a = null, int? b = null)
        {
            throw ThrowNotSupportedException(type, a, b);
        }
    }
}
