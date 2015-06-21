using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Kingdom.Data.Attributes;
using Kingdom.Data.Migrations;

namespace Kingdom.Data.Runners
{
    /// <summary>
    /// Migration runner base class.
    /// </summary>
    /// <typeparam name="TRunner"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class MigrationRunnerBase<TRunner, TValue>
        : IMigrationRunner<TRunner, TValue>
        where TValue : IComparable<TValue>
        where TRunner : MigrationRunnerBase<TRunner, TValue>
    {
        /// <summary>
        /// Gets the Context.
        /// </summary>
        protected DbContext Context { get; private set; }

        /// <summary>
        /// Gets the Assemblies from which to load the migrations.
        /// </summary>
        protected IEnumerable<Assembly> Assemblies { get; private set; }

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="types"></param>
        protected MigrationRunnerBase(DbConnection connection,
            params Type[] types)
            : this(connection, types.Select(x => x.Assembly).ToArray())
        {
        }

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="assemblies"></param>
        protected MigrationRunnerBase(DbConnection connection,
            params Assembly[] assemblies)
        {
            Context = new MigrationDbContext(connection, true);
            Assemblies = assemblies;
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~MigrationRunnerBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Migrations backing field.
        /// </summary>
        private IEnumerable<IMigration> _migrations;

        /// <summary>
        /// Gets the Migrations.
        /// </summary>
        private IEnumerable<IMigration> Migrations
        {
            get
            {
                TryLoad(out _migrations);
                return _migrations;
            }
        }

        /// <summary>
        /// Loads the <see cref="IMigration"/> implementations from the
        /// <see cref="Assemblies"/>.
        /// </summary>
        /// <param name="migrations"></param>
        /// <returns></returns>
        private bool TryLoad(out IEnumerable<IMigration> migrations)
        {
            var migrationBaseType = typeof (MigrationBase);

            var theActualTypes = Assemblies.SelectMany(x => x.GetTypes())
                .Where(t => !t.IsAbstract && migrationBaseType.IsAssignableFrom(t)).ToArray();

            var loaded = theActualTypes.Select(t => (IMigration)
                Activator.CreateInstance(t)).ToArray();

            Debug.Assert(loaded.Select(x => ((MigrationBase) x).Info.Attrib.GetType()).Distinct().Count() == 1,
                @"Migration versioning must be consistently applied.");

            //Then inject the Context.
            migrations = loaded.Select(x =>
            {
                ((MigrationBase) x).Context = Context;
                return x;
            }).ToArray();

            return migrations.Any();
        }

        /// <summary>
        /// Returns the applied migrations.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<VersionInfo> GetAppliedMigrations()
        {
            var set = Context.Set<VersionInfo>();
            return set.ToArray();
        }

        /// <summary>
        /// Returns the max applied Version.
        /// </summary>
        /// <returns></returns>
        protected long GetMaxAppliedVersion()
        {
            //Get the last known applied migration.
            var applied = GetAppliedMigrations().ToArray();

            return applied.Any()
                ? applied.Max(a => a.VersionId)
                : new Version(0, 0).ToLongId();
        }

        /// <summary>
        /// Up handler accepting a migration.
        /// </summary>
        /// <param name="migration"></param>
        private void UpHandler(IMigration migration)
        {
            var local = (MigrationBase) migration;
            local.Up();
            var set = Context.Set<VersionInfo>();
            var info = local.Info.GetVersion();
            set.Add(info);
            Context.SaveChanges();
        }

        /// <summary>
        /// Down handler accepting a migration.
        /// </summary>
        /// <param name="migration"></param>
        private void DownHandler(IMigration migration)
        {
            var local = (MigrationBase) migration;
            local.Down();
            var set = Context.Set<VersionInfo>();
            var info = set.SingleOrDefault(x => x.VersionId == local.Info.Attrib.Id);
            set.Remove(info);
            Context.SaveChanges();
        }

        /// <summary>
        /// Migrates Up all versions.
        /// </summary>
        public void Up()
        {
            //TODO: could look for Migrations that have not been applied? careful of infilled migrations (?)

            Migrate(
                UpHandler,
                () =>
                {
                    /* TODO: I don't think it's a problem to solve if there are new/old interleaved
                     * migrations: there's only so much this can do to facilitate before that's part
                     * of the self-discipline aspect */

                    var lastId = GetMaxAppliedVersion();

                    //Obtain the migrations that are eligible since the last migration.
                    var migrations = Migrations.Where(m => ((MigrationBase) m).Info.Attrib.Id > lastId)
                        .OrderBy(m => ((MigrationBase) m).Info.Attrib.Id).ToArray();

                    return migrations;
                });
        }

        /// <summary>
        /// Migrates Down all versions.
        /// </summary>
        public void Down()
        {
            Migrate(
                DownHandler,
                () =>
                {
                    //Get the Ids that have been applied.
                    var ids = GetAppliedMigrations().ToArray().Select(x => x.VersionId).ToArray();

                    //Obtain the migrations that correspond to those Ids.
                    var migrations = Migrations.Where(m => ids.Contains(((MigrationBase) m).Info.Attrib.Id))
                        .OrderByDescending(m => ((MigrationBase) m).Info.Attrib.Id).ToArray();

                    return migrations;
                });
        }

        /// <summary>
        /// Migrates Up to the <see cref="maxValue"/>.
        /// </summary>
        /// <param name="maxValue"></param>
        public void Up(TValue maxValue)
        {
            Migrate(
                UpHandler,
                () =>
                {
                    //Obtain the range of migrations that are eligible for downgrade.
                    var migrations = Migrations.Where(m => ((TValue) ((MigrationBase) m).Info.Attrib.Value)
                        .CompareTo(maxValue) < 0).ToArray();

                    //Get the Ids of the applied migrations.
                    var ids = GetAppliedMigrations().ToArray().Select(x => x.VersionId).ToArray();

                    //Refine the actual set of migrations.
                    var theMigrations = migrations.Where(m => !ids.Contains(((MigrationBase) m).Info.Attrib.Id))
                        .OrderBy(m => ((MigrationBase) m).Info.Attrib.Id).ToArray();

                    return theMigrations;
                });
        }

        /// <summary>
        /// Migrates Down to the <see cref="minValue"/>.
        /// </summary>
        /// <param name="minValue"></param>
        public void Down(TValue minValue)
        {
            Migrate(
                DownHandler,
                () =>
                {
                    //Obtain the range of migrations that are eligible for downgrade.
                    var migrations = Migrations.Where(m => ((TValue) ((MigrationBase) m).Info.Attrib.Value)
                        .CompareTo(minValue) > 0).ToArray();

                    //Get the Ids of the applied migrations.
                    var ids = GetAppliedMigrations().ToArray().Select(x => x.VersionId).ToArray();

                    //Refine the actual set of migrations.
                    var theMigrations = migrations.Where(m => ids.Contains(((MigrationBase) m).Info.Attrib.Id))
                        .OrderByDescending(m => ((MigrationBase) m).Info.Attrib.Id).ToArray();

                    return theMigrations;
                });
        }

        /// <summary>
        /// Runs the runner given a delegated action.
        /// </summary>
        /// <param name="runner"></param>
        public virtual void Run(Action<TRunner> runner)
        {
            runner((TRunner) this);
        }

        /// <summary>
        /// Performs all migrations.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="get"></param>
        private void Migrate(MigrationActionDelegate action,
            Func<IEnumerable<IMigration>> get)
        {
            //Confirmed that user security correctly reflects a created database.
            Context.Database.CreateIfNotExists();

            var migrations = get();

            foreach (var m in migrations)
                Migrate(m, action);
        }

        /// <summary>
        /// Performs each individual migration.
        /// </summary>
        /// <param name="migration"></param>
        /// <param name="action"></param>
        private void Migrate(IMigration migration,
            MigrationActionDelegate action)
        {
            using (var transaction = Context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    action(migration);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        #region Disposable Members

        /// <summary>
        /// Disposed backing field.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!disposing || _disposed) return;
            Assemblies = null;
            Context.Dispose();
            Context = null;
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            _disposed = true;
        }

        #endregion
    }
}
