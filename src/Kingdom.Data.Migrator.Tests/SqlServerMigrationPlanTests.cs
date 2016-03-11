using System;
using NUnit.Framework;
using Kingdom.Data.Planners;
using Kingdom.Data.Runners;

namespace Kingdom.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlServerMigrationPlanTests : MigratorTestsBase
    {
        /// <summary>
        /// Establishes a simple migration plan for test purposes.
        /// </summary>
        public class SqlServerMigrationPlan : MigrationPlanBase<SqlServerMigrationRunner<Version>, Version>
        {
            public SqlServerMigrationPlan(string connectionString,
                MigrationRunnerFactory<SqlServerMigrationRunner<Version>, Version> runnerFactory,
                Action<SqlServerMigrationRunner<Version>> run)
                : base(connectionString, runnerFactory, run)
            {
            }
        }

        /// <summary>
        /// Verifies that the migration plan runs.
        /// </summary>
        [Test]
        public virtual void VerifyThatMigrationPlanRuns()
        {
            //TODO: This is not really saving us much...
            using (new SqlServerMigrationPlan(ConnectionString,
                cs => new SqlServerMigrationRunner<Version>(cs, typeof (SqlServerMigrationPlan)),
                runner =>
                {
                    runner.Down();
                    runner.Up();
                    runner.Down();
                    runner.Up();
                }))
            {
                //TODO: we can check anything after this runs?
            }
        }
    }
}
