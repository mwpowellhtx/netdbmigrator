using System;
using System.Data.SqlClient;
using System.Reflection;

namespace Kingdom.Data.Runners
{
    public class SqlServerMigrationRunner<TValue>
        : AbstractMigrationRunner<TValue>
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
