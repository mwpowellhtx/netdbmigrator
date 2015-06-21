using System;
using Kingdom.Data.Runners;
using NUnit.Framework;

namespace Kingdom.Data.Migrator.Tests
{
    public class SqlServerMigratorTests : MigratorTestsBase
    {
        /// <summary>
        /// Verifies that a migration runner runs.
        /// </summary>
        [Test]
        public virtual void VerifyThatMigrationRunnerRuns()
        {
            //TODO: may pull together a series of full and partial downgrades and upgrades for test purposes.
            // Run all the migrations discovered in the assembly rooting this test fixture.
            using (var runner = new SqlServerMigrationRunner<Version>(ConnectionString,
                typeof (SqlServerMigratorTests)))
            {
                runner.Down();
                runner.Up();
                runner.Down();
                runner.Up();
            }
        }
    }
}
