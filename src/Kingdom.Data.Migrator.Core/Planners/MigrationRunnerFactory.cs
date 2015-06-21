using System;
using Kingdom.Data.Runners;

namespace Kingdom.Data.Planners
{
    /// <summary>
    /// Provides factory creation of a <typeparamref name="TRunner"/>.
    /// </summary>
    /// <typeparam name="TRunner"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public delegate TRunner MigrationRunnerFactory<out TRunner, in TValue>(string connectionString)
        where TValue : IComparable<TValue>
        where TRunner : IMigrationRunner<TRunner, TValue>;
}
