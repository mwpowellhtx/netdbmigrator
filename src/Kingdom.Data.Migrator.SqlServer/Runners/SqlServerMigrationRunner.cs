using System;
using System.Data.SqlClient;
using System.Reflection;

namespace Kingdom.Data.Runners
{
    /// <summary>
    /// SqlServer migration runner. Specify the <see cref="TValue"/> that will be used to organize
    /// your migrations. This may be either <see cref="Version"/> or <see cref="DateTime"/>.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class SqlServerMigrationRunner<TValue>
        : MigrationRunnerBase<SqlServerMigrationRunner<TValue>, TValue>
        where TValue : IComparable<TValue>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="types"></param>
        public SqlServerMigrationRunner(string connectionString,
            params Type[] types)
            : base(new SqlConnection(connectionString), types)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="assemblies"></param>
        public SqlServerMigrationRunner(string connectionString,
            params Assembly[] assemblies)
            : base(new SqlConnection(connectionString), assemblies)
        {
        }
    }
}
