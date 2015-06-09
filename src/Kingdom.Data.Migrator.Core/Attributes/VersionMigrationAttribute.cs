using System;
using System.Linq;

namespace Kingdom.Data.Attributes
{
    /// <summary>
    /// Version migration attribute provides <see cref="Version"/> based migration details.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class VersionMigrationAttribute : AbstractMigrationAttribute
    {
        /// <summary>
        /// Gets the <see cref="AbstractMigrationAttribute.Value"/>
        /// as a <see cref="System.Version"/>.
        /// </summary>
        /// <see cref="AbstractMigrationAttribute.Value"/>
        public Version Version
        {
            get { return (Version) Value; }
        }

        /// <summary>
        /// Gets an Id based on the <see cref="Version"/>.
        /// </summary>
        internal override long Id
        {
            get { return Version.ToLongId(); }
        }

        /// <summary>
        /// Gets the Kind: <see cref="AttributeKind.Version"/>.
        /// </summary>
        internal override AttributeKind Kind
        {
            get { return AttributeKind.Version; }
        }

        /// <summary>
        /// Gets the Text from the <see cref="Version"/>.
        /// </summary>
        internal override string Text
        {
            get { return Version.ToString(); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="build"></param>
        /// <param name="revision"></param>
        public VersionMigrationAttribute(int major, int minor, int build = 0, int revision = 0)
            : base(new Version(major, minor, build, revision))
        {
        }

        /// <summary>
        /// Verifies that the attribute is valid for use.
        /// </summary>
        internal override void Verify()
        {
            var values = new[]
            {
                Version.Major,
                Version.Minor,
                Version.Build,
                Version.Revision
            };

            if (values.Any(x => x > 9999))
                throw new InvalidOperationException(@"Version fields must be 9999 or less.");
        }
    }

    internal static class VersionExtensionMethods
    {
        //TODO: put a couple of unit tests in just to verify this functionality.
        /// <summary>
        /// Returns a long Id based on the <see cref="version"/>.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        internal static long ToLongId(this Version version)
        {

            return ReferenceEquals(null, version)
                ? 0
                : long.Parse(string.Format(@"{0:D4}{1:D4}{2:D4}{3:D4}",
                    version.Major.ToNormalizedPart(),
                    version.Minor.ToNormalizedPart(),
                    version.Build.ToNormalizedPart(),
                    version.Revision.ToNormalizedPart()));
        }

        /// <summary>
        /// Returns a normalized part corresponding to the <see cref="value"/>. This means
        /// returning zero for negative values while allowing non negative values pass through.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static int ToNormalizedPart(this int value)
        {
            return value < 0 ? 0 : value;
        }
    }
}
