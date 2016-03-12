using System;

namespace Kingdom.Data
{
    internal static class ExceptionHandlingExtensionMethods
    {
        public static Exception ThrowNotSupportedException(this object root, string message)
        {
            return new NotSupportedException(message);
        }

        public static Exception ThrowNotSupportedException(this object root, Func<string> message)
        {
            return ThrowNotSupportedException(root, message());
        }
    }
}
