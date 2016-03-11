using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Kingdom.Data
{
    using NUnit.Framework;

    public class FluentSqlServerAlterTableTests : FluentlyAlterTestFixtureBase<SqlServerFluentAlterTable>
    {
        private static IColumn CreateColumn(string columnName)
        {
            return new SqlServerColumn { Name = NamePath.Create(columnName) };
        }

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
            const SqlDbType floatType = SqlDbType.Float;

            yield return new TestCaseData(fooNamePath(), (CheckType?) null, BuildColumns(
                CreateColumn(intName, intType).Add(new NullableColumnAttribute())
                , CreateColumn(bufferName, varBinaryType).Add(new PrecisionColumnAttribute {Precision = 16})
                , CreateColumn(floatName, floatType).Add(new PrecisionColumnAttribute {Precision = 42},
                    new ScaleColumnAttribute {Scale = 3})).ToValuesFixture()
                , "ALTER TABLE [dbo].[foo] ADD [myInt] [INT] NOT NULL, [myBuffer] [VARBINARY](16), [myFloat] [FLOAT](42);"
                );

            yield return new TestCaseData(barNamePath(), CheckType.Check, BuildColumns(
                CreateColumn(intName, intType).Add(new NullableColumnAttribute())
                , CreateColumn(bufferName, varBinaryType).Add(new NullableColumnAttribute {CanBeNull = true}
                    , new PrecisionColumnAttribute {Precision = 16})
                , CreateColumn(floatName, floatType).Add(new PrecisionColumnAttribute {Precision = 42},
                    new ScaleColumnAttribute {Scale = 3})).ToValuesFixture()
                , "ALTER TABLE [dbo].[bar] WITH CHECK ADD [myInt] [INT] NOT NULL, [myBuffer] [VARBINARY](16) NULL, [myFloat] [FLOAT](42);"
                );

            yield return new TestCaseData(fizNamePath(), CheckType.NoCheck, BuildColumns(
                CreateColumn(intName, intType).Add(new NullableColumnAttribute())
                , CreateColumn(bufferName, varBinaryType).Add(new PrecisionColumnAttribute {Precision = 16})
                , CreateColumn(floatName, floatType).Add(new PrecisionColumnAttribute {Precision = 42}
                    , new ScaleColumnAttribute {Scale = 3})).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] WITH NOCHECK ADD [myInt] [INT] NOT NULL, [myBuffer] [VARBINARY](16), [myFloat] [FLOAT](42);"
                );
        }

        private static IEnumerable<TestCaseData> GetAlterTableDropColumnsTestCases()
        {
            // There's only so much we can do to help with the verbosity.
            Func<INamePath> fooNamePath = () => NamePath.Create("dbo", "foo");
            Func<INamePath> barNamePath = () => NamePath.Create("dbo", "bar");
            Func<INamePath> fizNamePath = () => NamePath.Create("dbo", "fiz");

            const string intName = "myInt";
            const string bufferName = "myBuffer";
            const string floatName = "myFloat";

            yield return new TestCaseData(fooNamePath(), (CheckType?) null, BuildColumns(
                CreateColumn(intName), CreateColumn(bufferName), CreateColumn(floatName)).ToValuesFixture()
                , "ALTER TABLE [dbo].[foo] DROP COLUMN [myInt], [myBuffer], [myFloat];"
                );

            yield return new TestCaseData(barNamePath(), CheckType.Check, BuildColumns(
                CreateColumn(intName), CreateColumn(bufferName), CreateColumn(floatName)).ToValuesFixture()
                , "ALTER TABLE [dbo].[bar] WITH CHECK DROP COLUMN [myInt], [myBuffer], [myFloat];"
                );

            yield return new TestCaseData(fizNamePath(), CheckType.NoCheck, BuildColumns(
                CreateColumn(intName), CreateColumn(bufferName), CreateColumn(floatName)).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] WITH NOCHECK DROP COLUMN [myInt], [myBuffer], [myFloat];"
                );
        }

        private IEnumerable<TestCaseData> AlterTableAddColumnsTestCases
        {
            get { return GetAlterTableAddColumnsTestCases(); }
        }

        private IEnumerable<TestCaseData> AlterTableDropColumnsTestCases
        {
            get { return GetAlterTableDropColumnsTestCases(); }
        }

        private static class TestCases
        {
            internal const string AlterTableAddColumnsTestCases = "AlterTableAddColumnsTestCases";
            internal const string AlterTableDropColumnsTestCases = "AlterTableDropColumnsTestCases";
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

        [Test]
        [TestCaseSource(TestCases.AlterTableDropColumnsTestCases)]
        public void CanAlterTableDropColumns(INamePath tableName, CheckType? withCheck, IValuesFixture<IColumn> columns, string expectedSql)
        {
            // The real gains are realized here when we finally fluently build the alter statement.
            var actualSql = Alter.Table(tableName, withCheck).Drop(columns.Values.ToArray()).ToString();

            Console.WriteLine("CanAlterTableDropColumns SQL: {0}", actualSql);

            Assert.That(actualSql, Is.EqualTo(expectedSql).IgnoreCase);
        }
    }
}
