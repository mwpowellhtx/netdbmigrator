using System;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;

namespace Kingdom.Data.Migrator.Tests
{
    using Constraint = NUnit.Framework.Constraints.Constraint;

    public abstract class MigratorTestsBase : TestFixtureBase
    {
        /// <summary>
        /// DataSource: @"localhost"
        /// </summary>
        private const string DataSource = @"localhost";

        /// <summary>
        /// Gets or sets the DatabaseGuid.
        /// </summary>
        private Guid DatabaseGuid { get; set; }

        /// <summary>
        /// Gets the InitialCatalog.
        /// </summary>
        /// <see cref="DatabaseGuid"/>
        private string InitialCatalog
        {
            get { return DatabaseGuid.ToString(@"D").ToLower(); }
        }

        //TODO: would be better to use an embedded Sql provider, but for now this will work just fine depending on a default localhost instance
        /// <summary>
        /// Gets the ConnectionString.
        /// </summary>
        protected string ConnectionString
        {
            get
            {
                return string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=SSPI",
                    DataSource, InitialCatalog);
            }
        }

        /// <summary>
        /// Gets the MasterConnectionString.
        /// </summary>
        private static string MasterConnectionString
        {
            get
            {
                return string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=SSPI",
                    DataSource, @"master");
            }
        }

        /// <summary>
        /// Sets up prior to running all unit tests.
        /// Builds a migration database for use throughout the unit tests.
        /// </summary>
        public override void TestFixtureSetUp()
        {
            base.TestFixtureSetUp();

            // Verify that we can operate in the Master database.
            using (GetSqlConnection(MasterConnectionString, true))
            {
            }

            // Setup the Database identifier.
            DatabaseGuid = Guid.NewGuid();

            // Verify that the migration database does not exist.
            using (GetSqlConnection(ConnectionString, false))
            {
            }
        }

        /// <summary>
        /// Tears down after running all unit tests.
        /// There should not be any remnants of the migration database when the tests are completed.
        /// </summary>
        public override void TestFixtureTearDown()
        {
            // We expect that a connection can be made at this moment.
            using (GetSqlConnection(ConnectionString, true))
            {
            }

            // Then it is safe to drop the migration database altogether.
            DropMigrationDataSource(MasterConnectionString, InitialCatalog);

            // Verify that the database did indeed drop.
            using (GetSqlConnection(ConnectionString, false))
            {
            }

            base.TestFixtureTearDown();
        }

        /// <summary>
        /// Drops the migration DataSource (database) using the <see cref="MasterConnectionString"/>
        /// as a reference catalog to do the opertion.
        /// </summary>
        /// <param name="masterConnectionString"></param>
        /// <param name="initialCatalog"></param>
        private static void DropMigrationDataSource(string masterConnectionString, string initialCatalog)
        {
            // Do some work in the Master database to drop the test database.
            using (var connection = GetSqlConnection(masterConnectionString, true))
            {
                Assert.That(connection, Is.Not.Null, @"Null connection");

                var actualState = connection.State;
                Assert.That(actualState, Is.EqualTo(ConnectionState.Open), @"Unexpected connection state");

                // This broader context prohibits from running in a transaction.
                ExecuteSqlCommand(connection, () => "ALTER DATABASE \"" + initialCatalog
                                                    + "\" SET SINGLE_USER WITH ROLLBACK IMMEDIATE");

                ExecuteSqlCommand(connection, () => "DROP DATABASE \"" + initialCatalog + "\"");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        private static void ExecuteSqlCommand(SqlConnection connection, Func<string> commandText)
        {
            var disposed = false;
            var completed = false;

            Assert.That(connection, Is.Not.Null, @"Null connection");

            var actualState = connection.State;
            Assert.That(actualState, Is.EqualTo(ConnectionState.Open), @"Unexpected connection state");

            /* Cannot perform this in a transaction (I tried). Not that commands can't run in a transaction,
             * they probably should. But the broader context prohibits that from being possible. */
            using (var command = connection.CreateCommand())
            {
                Assert.That(command.Connection, Is.SameAs(connection), @"invalid command connection");

                command.Disposed += delegate { disposed = true; };

                command.CommandText = commandText();

                TestDelegate runner = () =>
                {
                    Assert.That(!disposed, @"Command disposed prior to executing");
                    command.ExecuteNonQuery();
                    completed = true;
                };

                Assert.That(runner, Throws.Nothing, @"Command failed");
            }

            Assert.That(disposed, @"Failed to dispose command");
            Assert.That(completed, @"Command did not complete");
        }

        /// <summary>
        /// Returns a <see cref="SqlConnection"/> corresponding with the <see cref="ConnectionString"/>
        /// verifying the indicated <see cref="existing"/> condition.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="existing"></param>
        /// <returns></returns>
        private static SqlConnection GetSqlConnection(string connectionString, bool existing)
        {
            Exception exception = null;

            var result = new SqlConnection(connectionString);
            ConnectionState? actualState = null;

            TestDelegate get = () =>
            {
                try
                {
                    result.OpenAsync().Wait();
                    actualState = result.State;
                }
                catch (Exception ex)
                {
                    actualState = result.State;
                    exception = ex;
                    throw;
                }
            };

            Assert.That(get,
                existing
                    ? Throws.Nothing
                    : (Constraint) Throws.InstanceOf<AggregateException>());

            if (existing)
            {
                Assert.That(exception, Is.Null);
                Assert.That(actualState, Is.Not.Null.And.EqualTo(ConnectionState.Open));
            }
            else
            {
                Assert.That(exception, Is.Not.Null);
                Assert.That(exception.InnerException, Is.Not.Null);
                Assert.That(exception.InnerException, Is.InstanceOf<SqlException>());

                Assert.That(actualState, Is.Null.Or.Not.EqualTo(ConnectionState.Open));
            }

            return result;
        }
    }
}
