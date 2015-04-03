using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Kingdom.Data.Migrations;

namespace Kingdom.Data.Runners
{
    public abstract class AbstractMigrationRunner<TValue>
        : IMigrationRunner<TValue>
        where TValue : IComparable<TValue>
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
        protected AbstractMigrationRunner(DbConnection connection,
            params Type[] types)
            : this(connection, types.Select(x => x.Assembly).ToArray())
        {
        }

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="assemblies"></param>
        protected AbstractMigrationRunner(DbConnection connection,
            params Assembly[] assemblies)
        {
            Context = new MigrationContext(connection, true);
            Assemblies = assemblies;
        }

        /// <summary>
        /// Migrations backing field.
        /// </summary>
        private IEnumerable<AbstractMigration> _migrations;

        /// <summary>
        /// Gets the Migrations.
        /// </summary>
        private IEnumerable<AbstractMigration> Migrations
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
        private bool TryLoad(out IEnumerable<AbstractMigration> migrations)
        {
            var migrationType = typeof (AbstractMigration);

            var theActualTypes = Assemblies.SelectMany(x => x.GetTypes())
                .Where(t => !t.IsAbstract && migrationType.IsAssignableFrom(t)).ToArray();

            var loaded = theActualTypes.Select(t => (AbstractMigration)
                Activator.CreateInstance(t)).ToArray();

            Debug.Assert(loaded.Select(x => x.Info.Attrib.GetType()).Distinct().Count() == 1,
                @"Migration versioning must be consistently applied.");

            //Then inject the Context.
            migrations = loaded.Select(x =>
            {
                x.Context = Context;
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

        private void UpHandler(AbstractMigration migration)
        {
            migration.Up();
            var set = Context.Set<VersionInfo>();
            var info = migration.Info.GetVersion();
            set.Add(info);
            Context.SaveChanges();
        }

        private void DownHandler(AbstractMigration migration)
        {
            migration.Down();
            var set = Context.Set<VersionInfo>();
            var info = set.SingleOrDefault(x => x.VersionId == migration.Info.Attrib.Id);
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
                    //Get the last known applied migration.
                    var lastId = GetAppliedMigrations().ToArray().Max(x => x.VersionId);

                    //Obtain the migrations that are eligible since the last migration.
                    var migrations = Migrations.Where(m => m.Info.Attrib.Id > lastId)
                        .OrderBy(m => m.Info.Attrib.Id).ToArray();

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
                    var migrations = Migrations.Where(m => ids.Contains(m.Info.Attrib.Id))
                        .OrderByDescending(x => x.Info.Attrib.Id).ToArray();

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
                    var migrations = Migrations.Where(m => ((TValue) m.Info.Attrib.Value)
                        .CompareTo(maxValue) < 0).ToArray();

                    //Get the Ids of the applied migrations.
                    var ids = GetAppliedMigrations().ToArray().Select(x => x.VersionId).ToArray();

                    //Refine the actual set of migrations.
                    var theMigrations = migrations.Where(m => !ids.Contains(m.Info.Attrib.Id))
                        .OrderBy(x => x.Info.Attrib.Id).ToArray();

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
                    var migrations = Migrations.Where(m => ((TValue) m.Info.Attrib.Value)
                        .CompareTo(minValue) > 0).ToArray();

                    //Get the Ids of the applied migrations.
                    var ids = GetAppliedMigrations().ToArray().Select(x => x.VersionId).ToArray();

                    //Refine the actual set of migrations.
                    var theMigrations = migrations.Where(m => ids.Contains(m.Info.Attrib.Id))
                        .OrderByDescending(x => x.Info.Attrib.Id).ToArray();

                    return theMigrations;
                });
        }

        private void Migrate(MigrationActionDelegate action,
            Func<IEnumerable<AbstractMigration>> get)
        {
            //Confirmed that user security correctly reflects a created database.
            Context.Database.CreateIfNotExists();

            var migrations = get();
            foreach (var m in migrations)
                Migrate(m, action);
        }

        private void Migrate(AbstractMigration migration,
            MigrationActionDelegate action)
        {
            using (var transaction = Context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    action(migration);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
