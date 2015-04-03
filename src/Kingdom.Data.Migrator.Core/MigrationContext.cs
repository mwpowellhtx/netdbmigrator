using System.Data.Common;
using System.Data.Entity;

namespace Kingdom.Data
{
    public class MigrationContext : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="existingConnection"></param>
        /// <param name="contextOwnsConnection"></param>
        public MigrationContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Properties<string>()
            //    .Configure(c => c.HasMaxLength(int.MaxValue));

            modelBuilder.Entity<VersionInfo>()
                .ToTable(typeof (VersionInfo).Name);
        }
    }
}
