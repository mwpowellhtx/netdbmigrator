using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Kingdom.Data
{
    using NUnit.Framework;

    public class FluentSqlServerAlterTableTests : FluentlyAlterTestFixtureBase<SqlServerFluentAlterTable>
    {
        private static IColumn CreateColumn(string columnName, SqlDbType type)
        {
            return new SqlServerColumn {Name = NamePath.Create(columnName), Type = type};
        }

        private static IEnumerable<IColumn> BuildColumns(params IColumn[] columns)
        {
            return columns.ToList();
        }

        private static IEnumerable<TestCaseData> GetAlterTableAddColumnsTestCases()
        {
            // There's only so much we can do to help with the verbosity.
            Func<INamePath> fooNamePath = () => NamePath.Create("dbo", "foo");
            Func<INamePath> barNamePath = () => NamePath.Create("dbo", "bar");
            Func<INamePath> fizNamePath = () => NamePath.Create("dbo", "fiz");

            const string intName = "myInt";
            const string bufferName = "myBuffer";
            const string floatName = "myFloat";

            const SqlDbType intType = SqlDbType.Int;
            const SqlDbType varBinaryType = SqlDbType.VarBinary;
            const SqlDbType floarType = SqlDbType.Float;

            yield return new TestCaseData(fooNamePath(), (CheckType?) null, BuildColumns(
                CreateColumn(intName, intType).Add(new NullableColumnAttribute())
                , CreateColumn(bufferName, varBinaryType).Add(new PrecisionColumnAttribute {Precision = 16})
                , CreateColumn(floatName, floarType).Add(new PrecisionColumnAttribute {Precision = 42},
                    new ScaleColumnAttribute {Scale = 3})).ToValuesFixture()
                , "ALTER TABLE [dbo].[foo] ADD [myInt] [INT] NOT NULL, [myBuffer] [VARBINARY](16), [myFloat] [FLOAT](42);"
                );

            yield return new TestCaseData(barNamePath(), CheckType.Check, BuildColumns(
                CreateColumn(intName, intType).Add(new NullableColumnAttribute())
                , CreateColumn(bufferName, varBinaryType).Add(new NullableColumnAttribute {CanBeNull = true}
                    , new PrecisionColumnAttribute {Precision = 16})
                , CreateColumn(floatName, floarType).Add(new PrecisionColumnAttribute {Precision = 42},
                    new ScaleColumnAttribute {Scale = 3})).ToValuesFixture()
                , "ALTER TABLE [dbo].[bar] WITH CHECK ADD [myInt] [INT] NOT NULL, [myBuffer] [VARBINARY](16) NULL, [myFloat] [FLOAT](42);"
                );

            yield return new TestCaseData(fizNamePath(), CheckType.NoCheck, BuildColumns(
                CreateColumn(intName, intType).Add(new NullableColumnAttribute())
                , CreateColumn(bufferName, varBinaryType).Add(new PrecisionColumnAttribute {Precision = 16})
                , CreateColumn(floatName, floarType).Add(new PrecisionColumnAttribute {Precision = 42}
                    , new ScaleColumnAttribute {Scale = 3})).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] WITH NOCHECK ADD [myInt] [INT] NOT NULL, [myBuffer] [VARBINARY](16), [myFloat] [FLOAT](42);"
                );
        }

        private IEnumerable<TestCaseData> AlterTableAddColumnsTestCases
        {
            get { return GetAlterTableAddColumnsTestCases(); }
        }

        private static class TestCases
        {
            internal const string AlterTableAddColumnsTestCases = "AlterTableAddColumnsTestCases";
        }

        [Test]
        [TestCaseSource(TestCases.AlterTableAddColumnsTestCases)]
        public void CanAlterTableAddColumns(INamePath tableName, CheckType? withCheck, IValuesFixture<IColumn> columns, string expectedSql)
        {
            // The real gains are realized here when we finally fluently build the alter statement.
            var actualSql = Alter.Table(tableName, withCheck).Add(columns.Values.ToArray()).ToString();

            Console.WriteLine("CanAlterTableAddColumns SQL: {0}", actualSql);

            Assert.That(actualSql, Is.EqualTo(expectedSql).IgnoreCase);
        }
    }
}
