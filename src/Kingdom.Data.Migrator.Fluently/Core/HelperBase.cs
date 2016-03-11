namespace Kingdom.Data
{
    /// <summary>
    /// Represents a helper base class.
    /// </summary>
    public interface IHelper
    {
    }

    //TODO: pretty sure these have been refactored, but will leave them in just for the moment.
    ///// <summary>
    ///// Represents a helper base class.
    ///// </summary>
    //public abstract class HelperBase : IHelper
    //{
    //    /// <summary>
    //    /// Performs the <paramref name="action"/> on the helper.
    //    /// </summary>
    //    /// <typeparam name="THelper"></typeparam>
    //    /// <param name="action"></param>
    //    /// <returns></returns>
    //    protected THelper HelperAction<THelper>(Action<THelper> action)
    //        where THelper : HelperBase
    //    {
    //        var helper = (THelper) this;
    //        action(helper);
    //        return helper;
    //    }

    //    /// <summary>
    //    /// Override to provide database provider specific output.
    //    /// </summary>
    //    /// <returns></returns>
    //    protected abstract string Build();

    //    /// <summary>
    //    /// Returns the string representation of the helper assisted output.
    //    /// </summary>
    //    /// <returns></returns>
    //    public override string ToString()
    //    {
    //        return Build();
    //    }

    //    /// <summary>
    //    /// Returns the <see cref="NotSupportedException"/>.
    //    /// </summary>
    //    /// <returns></returns>
    //    protected internal static Exception ThrowNotSupportedException()
    //    {
    //        return new NotSupportedException();
    //    }

    //    /// <summary>
    //    /// Returns the <see cref="NotSupportedException"/> corresponding to the <see cref="value"/>.
    //    /// </summary>
    //    /// <param name="value"></param>
    //    /// <param name="format"></param>
    //    /// <typeparam name="T"></typeparam>
    //    /// <returns></returns>
    //    protected static Exception ThrowNotSupportedException<T>(T value, Func<T, string> format = null)
    //    {
    //        format = format ?? (x => x.ToString());
    //        var message = string.Format("{0} is not supported.", format(value));
    //        return new NotSupportedException(message);
    //    }
    //}
}
