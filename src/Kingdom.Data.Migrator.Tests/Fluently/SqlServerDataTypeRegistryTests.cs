using System.Collections.Generic;
using System.Data;

namespace Kingdom.Data
{
    using NUnit.Framework;
    using TN = SqlServerDataTypeRegistryTests.TypeNames;

    public class SqlServerDataTypeRegistryTests : TestFixtureBase
    {
        private const int A = 11;
        private const int B = 22;
        private const int Max = int.MaxValue;

        internal static class TypeNames
        {
            // DbType mappings
            internal const string Char = "CHAR";
            internal const string NChar = "NCHAR";
            internal const string VarChar = "VARCHAR";
            internal const string NVarChar = "NVARCHAR";
            internal const string Bit = "BIT";
            internal const string TinyInt = "TINYINT";
            internal const string SmallInt = "SMALLINT";
            internal const string Int = "INT";
            internal const string BigInt = "BIGINT";
            internal const string Real = "REAL";
            internal const string Float = "FLOAT";
            internal const string Date = "DATE";
            internal const string DateTime = "DATETIME";
            internal const string DateTime2 = "DATETIME2";
            internal const string DateTimeOffset = "DATETIMEOFFSET";
            internal const string Time = "TIME";
            internal const string UniqueIdentifier = "UNIQUEIDENTIFIER";
            internal const string Binary = "BINARY";
            internal const string VarBinary = "VARBINARY";

            // SqlDbType mappings
        }

        private static string GetFormattedType(string typeName, int? precision = null)
        {
            return string.Format(@"[{0}]({1})", typeName,
                precision == null
                    ? string.Empty
                    : (precision == Max ? "MAX" : precision.ToString()));
        }

        private static IEnumerable<TestCaseData> GetDbTypeMappedTestCases()
        {
            yield return new TestCaseData(DbType.AnsiStringFixedLength, A, null, GetFormattedType(TN.Char, A));
            yield return new TestCaseData(DbType.AnsiStringFixedLength, null, A, GetFormattedType(TN.Char, A));
            yield return new TestCaseData(DbType.AnsiStringFixedLength, Max, null, GetFormattedType(TN.Char));
            yield return new TestCaseData(DbType.AnsiStringFixedLength, null, Max, GetFormattedType(TN.Char));

            yield return new TestCaseData(DbType.StringFixedLength, A, null, GetFormattedType(TN.NChar, A));
            yield return new TestCaseData(DbType.StringFixedLength, null, A, GetFormattedType(TN.NChar, A));
            yield return new TestCaseData(DbType.StringFixedLength, Max, null, GetFormattedType(TN.NChar));
            yield return new TestCaseData(DbType.StringFixedLength, null, Max, GetFormattedType(TN.NChar));

            yield return new TestCaseData(DbType.AnsiString, A, null, GetFormattedType(TN.VarChar, A));
            yield return new TestCaseData(DbType.AnsiString, null, A, GetFormattedType(TN.VarChar, A));
            yield return new TestCaseData(DbType.AnsiString, Max, null, GetFormattedType(TN.VarChar));
            yield return new TestCaseData(DbType.AnsiString, null, Max, GetFormattedType(TN.VarChar));

            yield return new TestCaseData(DbType.String, A, null, GetFormattedType(TN.NVarChar, A));
            yield return new TestCaseData(DbType.String, null, A, GetFormattedType(TN.NVarChar, A));
            yield return new TestCaseData(DbType.String, Max, null, GetFormattedType(TN.NVarChar));
            yield return new TestCaseData(DbType.String, null, Max, GetFormattedType(TN.NVarChar));

            yield return new TestCaseData(DbType.Boolean, A, null, GetFormattedType(TN.Bit));
            yield return new TestCaseData(DbType.Boolean, null, A, GetFormattedType(TN.Bit));
            yield return new TestCaseData(DbType.Boolean, A, B, GetFormattedType(TN.Bit));
            yield return new TestCaseData(DbType.Boolean, null, null, GetFormattedType(TN.Bit));

            yield return new TestCaseData(DbType.Byte, A, null, GetFormattedType(TN.TinyInt));
            yield return new TestCaseData(DbType.Byte, null, A, GetFormattedType(TN.TinyInt));
            yield return new TestCaseData(DbType.Byte, A, B, GetFormattedType(TN.TinyInt));
            yield return new TestCaseData(DbType.Byte, null, null, GetFormattedType(TN.TinyInt));

            yield return new TestCaseData(DbType.Int16, A, null, GetFormattedType(TN.SmallInt));
            yield return new TestCaseData(DbType.Int16, null, A, GetFormattedType(TN.SmallInt));
            yield return new TestCaseData(DbType.Int16, A, B, GetFormattedType(TN.SmallInt));
            yield return new TestCaseData(DbType.Int16, null, null, GetFormattedType(TN.SmallInt));

            yield return new TestCaseData(DbType.Int32, A, null, GetFormattedType(TN.Int));
            yield return new TestCaseData(DbType.Int32, null, A, GetFormattedType(TN.Int));
            yield return new TestCaseData(DbType.Int32, A, B, GetFormattedType(TN.Int));
            yield return new TestCaseData(DbType.Int32, null, null, GetFormattedType(TN.Int));

            yield return new TestCaseData(DbType.Int64, A, null, GetFormattedType(TN.BigInt));
            yield return new TestCaseData(DbType.Int64, null, A, GetFormattedType(TN.BigInt));
            yield return new TestCaseData(DbType.Int64, A, B, GetFormattedType(TN.BigInt));
            yield return new TestCaseData(DbType.Int64, null, null, GetFormattedType(TN.BigInt));

            yield return new TestCaseData(DbType.Single, A, null, GetFormattedType(TN.Real));
            yield return new TestCaseData(DbType.Single, null, A, GetFormattedType(TN.Real));
            yield return new TestCaseData(DbType.Single, A, B, GetFormattedType(TN.Real));
            yield return new TestCaseData(DbType.Single, null, null, GetFormattedType(TN.Real));

            //TODO: we may want use cases tests that work the 1-24, 25-53 precision.
            yield return new TestCaseData(DbType.Double, A, null, GetFormattedType(TN.Float, A));
            yield return new TestCaseData(DbType.Double, null, A, GetFormattedType(TN.Float, A));
            yield return new TestCaseData(DbType.Double, B, A, GetFormattedType(TN.Float, B));
            yield return new TestCaseData(DbType.Double, null, null, GetFormattedType(TN.Float));

            yield return new TestCaseData(DbType.Date, A, null, GetFormattedType(TN.Date));
            yield return new TestCaseData(DbType.Date, null, A, GetFormattedType(TN.Date));
            yield return new TestCaseData(DbType.Date, A, B, GetFormattedType(TN.Date));
            yield return new TestCaseData(DbType.Date, null, null, GetFormattedType(TN.Date));

            yield return new TestCaseData(DbType.DateTime, A, null, GetFormattedType(TN.DateTime));
            yield return new TestCaseData(DbType.DateTime, null, A, GetFormattedType(TN.DateTime));
            yield return new TestCaseData(DbType.DateTime, A, B, GetFormattedType(TN.DateTime));
            yield return new TestCaseData(DbType.DateTime, null, null, GetFormattedType(TN.DateTime));

            yield return new TestCaseData(DbType.DateTime2, A, null, GetFormattedType(TN.DateTime2, A));
            yield return new TestCaseData(DbType.DateTime2, null, A, GetFormattedType(TN.DateTime2, A));
            yield return new TestCaseData(DbType.DateTime2, B, A, GetFormattedType(TN.DateTime2, B));
            yield return new TestCaseData(DbType.DateTime2, null, null, GetFormattedType(TN.DateTime2));

            yield return new TestCaseData(DbType.DateTimeOffset, A, null, GetFormattedType(TN.DateTimeOffset, A));
            yield return new TestCaseData(DbType.DateTimeOffset, null, A, GetFormattedType(TN.DateTimeOffset, A));
            yield return new TestCaseData(DbType.DateTimeOffset, B, A, GetFormattedType(TN.DateTimeOffset, B));
            yield return new TestCaseData(DbType.DateTimeOffset, null, null, GetFormattedType(TN.DateTimeOffset));

            yield return new TestCaseData(DbType.Time, A, null, GetFormattedType(TN.Time, A));
            yield return new TestCaseData(DbType.Time, null, A, GetFormattedType(TN.Time, A));
            yield return new TestCaseData(DbType.Time, B, A, GetFormattedType(TN.Time, B));
            yield return new TestCaseData(DbType.Time, null, null, GetFormattedType(TN.Time));

            yield return new TestCaseData(DbType.Guid, A, null, GetFormattedType(TN.UniqueIdentifier));
            yield return new TestCaseData(DbType.Guid, null, A, GetFormattedType(TN.UniqueIdentifier));
            yield return new TestCaseData(DbType.Guid, B, A, GetFormattedType(TN.UniqueIdentifier));
            yield return new TestCaseData(DbType.Guid, null, null, GetFormattedType(TN.UniqueIdentifier));

            yield return new TestCaseData(DbType.Binary, A, null, GetFormattedType(TN.Binary, A));
            yield return new TestCaseData(DbType.Binary, null, A, GetFormattedType(TN.Binary, A));
            yield return new TestCaseData(DbType.Binary, B, A, GetFormattedType(TN.Binary, B));
            yield return new TestCaseData(DbType.Binary, null, null, GetFormattedType(TN.Binary));
        }

        private static IEnumerable<TestCaseData> GetSqlDbTypeMappedTestCases()
        {
            yield return new TestCaseData(SqlDbType.Char, A, null, GetFormattedType(TN.Char, A));
            yield return new TestCaseData(SqlDbType.Char, null, A, GetFormattedType(TN.Char, A));
            yield return new TestCaseData(SqlDbType.Char, Max, null, GetFormattedType(TN.Char, Max));
            yield return new TestCaseData(SqlDbType.Char, null, Max, GetFormattedType(TN.Char, Max));

            yield return new TestCaseData(SqlDbType.NChar, A, null, GetFormattedType(TN.NChar, A));
            yield return new TestCaseData(SqlDbType.NChar, null, A, GetFormattedType(TN.NChar, A));
            yield return new TestCaseData(SqlDbType.NChar, Max, null, GetFormattedType(TN.NChar, Max));
            yield return new TestCaseData(SqlDbType.NChar, null, Max, GetFormattedType(TN.NChar, Max));

            yield return new TestCaseData(SqlDbType.VarChar, A, null, GetFormattedType(TN.VarChar, A));
            yield return new TestCaseData(SqlDbType.VarChar, null, A, GetFormattedType(TN.VarChar, A));
            yield return new TestCaseData(SqlDbType.VarChar, Max, null, GetFormattedType(TN.VarChar, Max));
            yield return new TestCaseData(SqlDbType.VarChar, null, Max, GetFormattedType(TN.VarChar, Max));

            yield return new TestCaseData(SqlDbType.NVarChar, A, null, GetFormattedType(TN.NVarChar, A));
            yield return new TestCaseData(SqlDbType.NVarChar, null, A, GetFormattedType(TN.NVarChar, A));
            yield return new TestCaseData(SqlDbType.NVarChar, Max, null, GetFormattedType(TN.NVarChar, Max));
            yield return new TestCaseData(SqlDbType.NVarChar, null, Max, GetFormattedType(TN.NVarChar, Max));

            yield return new TestCaseData(SqlDbType.Bit, A, null, GetFormattedType(TN.Bit));
            yield return new TestCaseData(SqlDbType.Bit, null, A, GetFormattedType(TN.Bit));
            yield return new TestCaseData(SqlDbType.Bit, A, B, GetFormattedType(TN.Bit));
            yield return new TestCaseData(SqlDbType.Bit, null, null, GetFormattedType(TN.Bit));

            yield return new TestCaseData(SqlDbType.TinyInt, A, null, GetFormattedType(TN.TinyInt));
            yield return new TestCaseData(SqlDbType.TinyInt, null, A, GetFormattedType(TN.TinyInt));
            yield return new TestCaseData(SqlDbType.TinyInt, A, B, GetFormattedType(TN.TinyInt));
            yield return new TestCaseData(SqlDbType.TinyInt, null, null, GetFormattedType(TN.TinyInt));

            yield return new TestCaseData(SqlDbType.SmallInt, A, null, GetFormattedType(TN.SmallInt));
            yield return new TestCaseData(SqlDbType.SmallInt, null, A, GetFormattedType(TN.SmallInt));
            yield return new TestCaseData(SqlDbType.SmallInt, A, B, GetFormattedType(TN.SmallInt));
            yield return new TestCaseData(SqlDbType.SmallInt, null, null, GetFormattedType(TN.SmallInt));

            yield return new TestCaseData(SqlDbType.Int, A, null, GetFormattedType(TN.Int));
            yield return new TestCaseData(SqlDbType.Int, null, A, GetFormattedType(TN.Int));
            yield return new TestCaseData(SqlDbType.Int, A, B, GetFormattedType(TN.Int));
            yield return new TestCaseData(SqlDbType.Int, null, null, GetFormattedType(TN.Int));

            yield return new TestCaseData(SqlDbType.BigInt, A, null, GetFormattedType(TN.BigInt));
            yield return new TestCaseData(SqlDbType.BigInt, null, A, GetFormattedType(TN.BigInt));
            yield return new TestCaseData(SqlDbType.BigInt, A, B, GetFormattedType(TN.BigInt));
            yield return new TestCaseData(SqlDbType.BigInt, null, null, GetFormattedType(TN.BigInt));

            yield return new TestCaseData(SqlDbType.Real, A, null, GetFormattedType(TN.Real));
            yield return new TestCaseData(SqlDbType.Real, null, A, GetFormattedType(TN.Real));
            yield return new TestCaseData(SqlDbType.Real, A, B, GetFormattedType(TN.Real));
            yield return new TestCaseData(SqlDbType.Real, null, null, GetFormattedType(TN.Real));

            //TODO: we may want use cases tests that work the 1-24, 25-53 precision.
            yield return new TestCaseData(SqlDbType.Float, A, null, GetFormattedType(TN.Float, A));
            yield return new TestCaseData(SqlDbType.Float, null, A, GetFormattedType(TN.Float, A));
            yield return new TestCaseData(SqlDbType.Float, B, A, GetFormattedType(TN.Float, B));
            yield return new TestCaseData(SqlDbType.Float, null, null, GetFormattedType(TN.Float));

            yield return new TestCaseData(SqlDbType.Date, A, null, GetFormattedType(TN.Date));
            yield return new TestCaseData(SqlDbType.Date, null, A, GetFormattedType(TN.Date));
            yield return new TestCaseData(SqlDbType.Date, A, B, GetFormattedType(TN.Date));
            yield return new TestCaseData(SqlDbType.Date, null, null, GetFormattedType(TN.Date));

            yield return new TestCaseData(SqlDbType.DateTime, A, null, GetFormattedType(TN.DateTime));
            yield return new TestCaseData(SqlDbType.DateTime, null, A, GetFormattedType(TN.DateTime));
            yield return new TestCaseData(SqlDbType.DateTime, A, B, GetFormattedType(TN.DateTime));
            yield return new TestCaseData(SqlDbType.DateTime, null, null, GetFormattedType(TN.DateTime));

            yield return new TestCaseData(SqlDbType.DateTime2, A, null, GetFormattedType(TN.DateTime2, A));
            yield return new TestCaseData(SqlDbType.DateTime2, null, A, GetFormattedType(TN.DateTime2, A));
            yield return new TestCaseData(SqlDbType.DateTime2, B, A, GetFormattedType(TN.DateTime2, B));
            yield return new TestCaseData(SqlDbType.DateTime2, null, null, GetFormattedType(TN.DateTime2));

            yield return new TestCaseData(SqlDbType.DateTimeOffset, A, null, GetFormattedType(TN.DateTimeOffset, A));
            yield return new TestCaseData(SqlDbType.DateTimeOffset, null, A, GetFormattedType(TN.DateTimeOffset, A));
            yield return new TestCaseData(SqlDbType.DateTimeOffset, B, A, GetFormattedType(TN.DateTimeOffset, B));
            yield return new TestCaseData(SqlDbType.DateTimeOffset, null, null, GetFormattedType(TN.DateTimeOffset));

            yield return new TestCaseData(SqlDbType.Time, A, null, GetFormattedType(TN.Time, A));
            yield return new TestCaseData(SqlDbType.Time, null, A, GetFormattedType(TN.Time, A));
            yield return new TestCaseData(SqlDbType.Time, B, A, GetFormattedType(TN.Time, B));
            yield return new TestCaseData(SqlDbType.Time, null, null, GetFormattedType(TN.Time));

            yield return new TestCaseData(SqlDbType.UniqueIdentifier, A, null, GetFormattedType(TN.UniqueIdentifier));
            yield return new TestCaseData(SqlDbType.UniqueIdentifier, null, A, GetFormattedType(TN.UniqueIdentifier));
            yield return new TestCaseData(SqlDbType.UniqueIdentifier, B, A, GetFormattedType(TN.UniqueIdentifier));
            yield return new TestCaseData(SqlDbType.UniqueIdentifier, null, null, GetFormattedType(TN.UniqueIdentifier));

            yield return new TestCaseData(SqlDbType.Binary, A, null, GetFormattedType(TN.Binary, A));
            yield return new TestCaseData(SqlDbType.Binary, null, A, GetFormattedType(TN.Binary, A));
            yield return new TestCaseData(SqlDbType.Binary, B, A, GetFormattedType(TN.Binary, B));
            yield return new TestCaseData(SqlDbType.Binary, null, null, GetFormattedType(TN.Binary));

            yield return new TestCaseData(SqlDbType.VarBinary, A, null, GetFormattedType(TN.VarBinary, A));
            yield return new TestCaseData(SqlDbType.VarBinary, null, A, GetFormattedType(TN.VarBinary, A));
            yield return new TestCaseData(SqlDbType.VarBinary, B, A, GetFormattedType(TN.VarBinary, B));
            yield return new TestCaseData(SqlDbType.VarBinary, Max, null, GetFormattedType(TN.VarBinary, Max));
            yield return new TestCaseData(SqlDbType.VarBinary, null, Max, GetFormattedType(TN.VarBinary, Max));
            yield return new TestCaseData(SqlDbType.VarBinary, null, null, GetFormattedType(TN.VarBinary));
        }

        private IEnumerable<TestCaseData> DbTypeMappedTestCases
        {
            get { return GetDbTypeMappedTestCases(); }
        }

        private IEnumerable<TestCaseData> SqlDbTypeMappedTestCases
        {
            get { return GetSqlDbTypeMappedTestCases(); }
        }

        private static class TestCases
        {
            internal const string DbTypeMappedTestCases = "DbTypeMappedTestCases";
            internal const string SqlDbTypeMappedTestCases = "SqlDbTypeMappedTestCases";
        }

        [Test]
        [TestCaseSource(TestCases.DbTypeMappedTestCases)]
        public void VerifyDbTypeMappedTypes(DbType type, int? a, int? b, string expected)
        {
            VerifyDataTypeRegistry<SqlServerDataTypeRegistry>(type, a, b, expected);
        }

        [Test]
        [TestCaseSource(TestCases.SqlDbTypeMappedTestCases)]
        public void VerifySqlDbTypeMappedTypes(SqlDbType type, int? a, int? b, string expected)
        {
            VerifyDataTypeRegistry<SqlServerDataTypeRegistry>(type, a, b, expected);
        }

        private static void VerifyDataTypeRegistry<TRegistry>(DbType type, int? a, int? b, string expected)
            where TRegistry : DataTypeRegistryBase, new()
        {
            var typeString = new TRegistry().GetDbTypeString(type, a, b);
            Assert.That(typeString, Is.EqualTo(expected));
        }

        private static void VerifyDataTypeRegistry<TRegistry>(SqlDbType type, int? a, int? b, string expected)
            where TRegistry : SqlServerDataTypeRegistry, new()
        {
            var typeString = new TRegistry().GetDbTypeString(type, a, b);
            Assert.That(typeString, Is.EqualTo(expected));
        }
    }
}
