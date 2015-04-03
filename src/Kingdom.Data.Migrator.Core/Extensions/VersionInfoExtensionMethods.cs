using System;
using System.Globalization;
using Kingdom.Data.Attributes;

namespace Kingdom.Data.Extensions
{
    internal static class VersionInfoExtensionMethods
    {
        internal static DateTime? GetTimeStamp(this VersionInfo info)
        {
            if (ReferenceEquals(null, info)
                || !(info.AttributeKind == AttributeKind.TimeStamp
                     || info.AttributeType == typeof (TimeStampMigrationAttribute)))
            {
                return null;
            }

            DateTime value;

            var provider = CultureInfo.InvariantCulture;
            const DateTimeStyles style = DateTimeStyles.RoundtripKind;

            return DateTime.TryParseExact(info.Text, @"O", provider, style,
                out value)
                ? (DateTime?) value
                : null;
        }

        internal static Version GetVersion(this VersionInfo info)
        {
            if (ReferenceEquals(null, info)
                || !(info.AttributeKind == AttributeKind.Version
                     || info.AttributeType == typeof (VersionMigrationAttribute)))
            {
                return null;
            }

            Version value;

            return Version.TryParse(info.Text, out value)
                ? value
                : null;
        }
    }
}