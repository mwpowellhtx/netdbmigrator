using System.Reflection;
using Kingdom.Data.Attributes;
using Kingdom.Data.Migrations;

namespace Kingdom.Data
{
    public class MigrationInfo
    {
        /// <summary>
        /// Migration backing field.
        /// </summary>
        private IMigration _migration;

        /// <summary>
        /// Gets the Migration.
        /// </summary>
        public IMigration Migration
        {
            get { return _migration; }
            internal set
            {
                Attrib = null;
                _migration = value;
                if (ReferenceEquals(null, _migration)) return;
                var type = _migration.GetType();
                Attrib = type.GetCustomAttribute<AbstractMigrationAttribute>(false);
                Attrib.DecoratedType = type;
            }
        }

        /// <summary>
        /// Gets the Attrib.
        /// </summary>
        public AbstractMigrationAttribute Attrib { get; private set; }

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="migration"></param>
        internal MigrationInfo(IMigration migration)
        {
            Migration = migration;
        }

        /// <summary>
        /// Returns the <see cref="VersionInfo"/> corresponding with this
        /// <see cref="MigrationInfo"/>.
        /// </summary>
        /// <returns></returns>
        internal VersionInfo GetVersion()
        {
            var info = new VersionInfo();
            info.Configure(Migration, Attrib);
            return info;
        }
    }
}
