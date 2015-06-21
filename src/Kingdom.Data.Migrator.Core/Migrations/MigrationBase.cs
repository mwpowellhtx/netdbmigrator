using System;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Kingdom.Data.Migrations
{
    /// <summary>
    /// Migration base class.
    /// </summary>
    public abstract class MigrationBase : IMigration
    {
        /// <summary>
        /// Gets the Description.
        /// Default is <see cref="Type.FullName"/>.
        /// </summary>
        /// <see cref="Type.FullName"/>
        public virtual string Description
        {
            get { return GetType().FullName; }
        }

        /// <summary>
        /// Gets the Context.
        /// </summary>
        protected internal DbContext Context { get; internal set; }

        /// <summary>
        /// Gets the MigrationInfo.
        /// </summary>
        internal MigrationInfo Info { get; private set; }

        /// <summary>
        /// Protected Constructor
        /// </summary>
        protected MigrationBase()
        {
            Info = new MigrationInfo(this);
        }

        /// <summary>
        /// Runs the EmbeddedSql found at the <see cref="relativePath"/> as
        /// bounded by the parent <see cref="object.GetType()"/>.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="parameters"></param>
        protected void EmbeddedSql(string relativePath, params object[] parameters)
        {
            var type = GetType();
            using (var s = type.Assembly.GetManifestResourceStream(type, relativePath))
            {
                Debug.Assert(!ReferenceEquals(null, s),
                    string.Format(@"Embedded Sql expected at resource path {0}.{1}",
                        type.Namespace, relativePath));

                using (var r = new StreamReader(s))
                {
                    var sql = r.ReadToEnd();
                    Sql(sql, parameters);
                }
            }
        }

        /// <summary>
        /// Runs the <see cref="sql"/> with <see cref="parameters"/>.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected int? Sql(string sql, params object[] parameters)
        {
            const string sep = "\r\n";

            //Ignore the last line: GO
            var lines = sql.Split(sep[0], sep[1])
                .Where(x => x.Trim().Any()).ToArray();

            var cleaned = lines.Last().ToLower().Equals(@"go")
                ? string.Join(sep, lines.Take(lines.Count() - 1)).Trim()
                : string.Join(sep, lines).Trim();

            return !cleaned.Any()
                ? (int?) null
                : Context.Database.ExecuteSqlCommand(cleaned, parameters);
        }

        /// <summary>
        /// Upgrades the database.
        /// </summary>
        public abstract void Up();

        /// <summary>
        /// Downgrades the database.
        /// </summary>
        public abstract void Down();

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public virtual void Dispose()
        {
            Context = null; //Does not own the Context.
            Info.Migration = null;
            Info = null;
        }
    }
}
