using System;
using System.Collections.Generic;
using System.Linq;

namespace Kingdom.Data
{
    internal static class EnumerationExtensionMethods
    {
        public static IEnumerable<T> GetValues<T>(this object root)
        {
            return Enum.GetValues(typeof (T)).Cast<T>();
        }
    }
}
