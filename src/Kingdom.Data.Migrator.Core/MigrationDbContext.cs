using System.Data.Common;
using System.Data.Entity;

namespace Kingdom.Data
{
    /// <summary>
    /// Prepares a DbContext for use during the Migrations.
    /// </summary>
    public class MigrationDbContext : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="existingConnection"></param>
        /// <param name="contextOwnsConnection"></param>
        public MigrationDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        /// <summary>
        /// ModelCreating event handler.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /* Besides EF mechanisms we require one table be present in the repository for
             * versioning purposes. The end user is free to name everything else as he or
             * she sees fit. */

            modelBuilder.Entity<VersionInfo>()
                .ToTable(typeof (VersionInfo).Name);
        }
    }
}
