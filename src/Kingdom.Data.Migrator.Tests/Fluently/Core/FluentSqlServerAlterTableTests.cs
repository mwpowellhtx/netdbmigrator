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
            return new SqlServerColumn {Name = NamePath.Create(columnName)};
        }

        private static IColumn CreateColumn(string columnName, SqlDbType type)
        {
            return new SqlServerColumn {Name = NamePath.Create(columnName), Type = type};
        }

        private static IConstraint CreateConstraint<TConstraint>(
            string constraintName, Action<TConstraint> initialize)
            where TConstraint : class, IConstraint, new()
        {
            var result = new TConstraint {Name = NamePath.Create(constraintName)};
            initialize(result);
            return result;
        }

        private static IEnumerable<T> BuildEnumeration<T>(params T[] items)
        {
            return items.ToList();
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

            yield return new TestCaseData(fooNamePath(), (CheckType?) null, BuildEnumeration(
                CreateColumn(intName, intType).Add(new NullableColumnAttribute())
                , CreateColumn(bufferName, varBinaryType).Add(new PrecisionColumnAttribute {Value = 16})
                , CreateColumn(floatName, floatType).Add(new PrecisionColumnAttribute {Value = 42},
                    new ScaleColumnAttribute {Value = 3})).ToValuesFixture()
                , "ALTER TABLE [dbo].[foo] ADD [myInt] [INT] NOT NULL, [myBuffer] [VARBINARY](16), [myFloat] [FLOAT](42);"
                );

            yield return new TestCaseData(barNamePath(), CheckType.Check, BuildEnumeration(
                CreateColumn(intName, intType).Add(new NullableColumnAttribute())
                , CreateColumn(bufferName, varBinaryType).Add(new NullableColumnAttribute {Value = true}
                    , new PrecisionColumnAttribute {Value = 16})
                , CreateColumn(floatName, floatType).Add(new PrecisionColumnAttribute {Value = 42},
                    new ScaleColumnAttribute {Value = 3})).ToValuesFixture()
                , "ALTER TABLE [dbo].[bar] WITH CHECK ADD [myInt] [INT] NOT NULL, [myBuffer] [VARBINARY](16) NULL, [myFloat] [FLOAT](42);"
                );

            yield return new TestCaseData(fizNamePath(), CheckType.NoCheck, BuildEnumeration(
                CreateColumn(intName, intType).Add(new NullableColumnAttribute())
                , CreateColumn(bufferName, varBinaryType).Add(new PrecisionColumnAttribute {Value = 16})
                , CreateColumn(floatName, floatType).Add(new PrecisionColumnAttribute {Value = 42}
                    , new ScaleColumnAttribute {Value = 3})).ToValuesFixture()
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

            yield return new TestCaseData(fooNamePath(), (CheckType?) null, BuildEnumeration(
                CreateColumn(intName), CreateColumn(bufferName), CreateColumn(floatName)).ToValuesFixture()
                , "ALTER TABLE [dbo].[foo] DROP COLUMN [myInt], [myBuffer], [myFloat];"
                );

            yield return new TestCaseData(barNamePath(), CheckType.Check, BuildEnumeration(
                CreateColumn(intName), CreateColumn(bufferName), CreateColumn(floatName)).ToValuesFixture()
                , "ALTER TABLE [dbo].[bar] WITH CHECK DROP COLUMN [myInt], [myBuffer], [myFloat];"
                );

            yield return new TestCaseData(fizNamePath(), CheckType.NoCheck, BuildEnumeration(
                CreateColumn(intName), CreateColumn(bufferName), CreateColumn(floatName)).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] WITH NOCHECK DROP COLUMN [myInt], [myBuffer], [myFloat];"
                );
        }

        private static IEnumerable<TestCaseData> GetAlterTableAddConstraintsTestCases()
        {
            // There's only so much we can do to help with the verbosity.
            Func<INamePath> fooNamePath = () => NamePath.Create("dbo", "foo");
            Func<INamePath> fizNamePath = () => NamePath.Create("dbo", "fiz");

            const string barPrimaryKeyName = "PK_foo_bar";
            const string bazPrimaryKeyName = "PK_fiz_baz";

            const string intName = "myInt";
            const string floatName = "myFloat";

            const SortOrder ascending = SortOrder.Ascending;
            const SortOrder descending = SortOrder.Descending;
            const TableIndexType primaryKey = TableIndexType.PrimaryKey;
            const TableIndexType uniqueIndex = TableIndexType.UniqueIndex;
            const ClusteredType clustered = ClusteredType.Clustered;
            const ClusteredType nonClustered = ClusteredType.NonClustered;

            yield return new TestCaseData(fooNamePath(), (CheckType?) null, BuildEnumeration(
                CreateConstraint(barPrimaryKeyName, (SqlServerPrimaryKeyOrUniqueConstraint constraint) =>
                {
                    var column = CreateColumn(intName).Add(new SortOrderColumnAttribute {Value = ascending});
                    constraint.KeyColumns.Add(column);
                }).Add(new TableIndexConstraintAttribute {Value = primaryKey},
                    new ClusteredConstraintAttribute {Value = clustered})).ToValuesFixture()
                , "ALTER TABLE [dbo].[foo] ADD CONSTRAINT [PK_foo_bar] PRIMARY KEY CLUSTERED ([myInt] ASC);"
                );

            yield return new TestCaseData(fizNamePath(), (CheckType?) null, BuildEnumeration(
                CreateConstraint(bazPrimaryKeyName, (SqlServerPrimaryKeyOrUniqueConstraint constraint) =>
                {
                    var column = CreateColumn(floatName).Add(new SortOrderColumnAttribute {Value = descending});
                    constraint.KeyColumns.Add(column);
                }).Add(new TableIndexConstraintAttribute {Value = uniqueIndex},
                    new ClusteredConstraintAttribute {Value = nonClustered})).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] ADD CONSTRAINT [PK_fiz_baz] UNIQUE NONCLUSTERED ([myInt] DESC);"
                );
        }

        private static IEnumerable<TestCaseData> GetAlterTableDropConstraintsTestCases()
        {
            // There's only so much we can do to help with the verbosity.
            Func<INamePath> fooNamePath = () => NamePath.Create("dbo", "foo");
            Func<INamePath> fizNamePath = () => NamePath.Create("dbo", "fiz");

            const string barPrimaryKeyName = "PK_foo_bar";
            const string bazPrimaryKeyName = "PK_fiz_baz";

            const string intName = "myInt";
            const string floatName = "myFloat";

            yield return new TestCaseData(fooNamePath(), (CheckType?) null, BuildEnumeration(
                CreateConstraint(barPrimaryKeyName, (SqlServerPrimaryKeyOrUniqueConstraint c) =>
                    c.KeyColumns.Add(CreateColumn(intName)))).ToValuesFixture()
                , "ALTER TABLE [dbo].[foo] DROP CONSTRAINT [PK_foo_bar];"
                );

            yield return new TestCaseData(fizNamePath(), (CheckType?) null, BuildEnumeration(
                CreateConstraint(bazPrimaryKeyName, (SqlServerPrimaryKeyOrUniqueConstraint constraint) =>
                    constraint.KeyColumns.Add(CreateColumn(floatName)))).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] DROP CONSTRAINT [PK_fiz_baz];"
                );
        }

        /// <summary>
        /// Gets the AlterTableAddColumnsTestCases.
        /// It is showing unused, but it will be used by the test framework.
        /// </summary>
        /// <see cref="GetAlterTableAddColumnsTestCases"/>
        private IEnumerable<TestCaseData> AlterTableAddColumnsTestCases
        {
            get { return GetAlterTableAddColumnsTestCases(); }
        }

        /// <summary>
        /// Gets the AlterTableDropColumnsTestCases.
        /// It is showing unused, but it will be used by the test framework.
        /// </summary>
        /// <see cref="GetAlterTableDropColumnsTestCases"/>
        private IEnumerable<TestCaseData> AlterTableDropColumnsTestCases
        {
            get { return GetAlterTableDropColumnsTestCases(); }
        }

        /// <summary>
        /// Gets the AlterTableAddConstraintsTestCases.
        /// It is showing unused, but it will be used by the test framework.
        /// </summary>
        /// <see cref="GetAlterTableAddConstraintsTestCases"/>
        private IEnumerable<TestCaseData> AlterTableAddConstraintsTestCases
        {
            get { return GetAlterTableAddConstraintsTestCases(); }
        }

        /// <summary>
        /// Gets the AlterTableDropConstraintsTestCases.
        /// It is showing unused, but it will be used by the test framework.
        /// </summary>
        /// <see cref="GetAlterTableDropConstraintsTestCases"/>
        private IEnumerable<TestCaseData> AlterTableDropConstraintsTestCases
        {
            get { return GetAlterTableDropConstraintsTestCases(); }
        }

        private static class TestCases
        {
            internal const string AlterTableAddColumnsTestCases = "AlterTableAddColumnsTestCases";
            internal const string AlterTableDropColumnsTestCases = "AlterTableDropColumnsTestCases";
            internal const string AlterTableAddConstraintsTestCases = "AlterTableAddConstraintsTestCases";
            internal const string AlterTableDropConstraintsTestCases = "AlterTableDropConstraintsTestCases";
        }

        [Test]
        [TestCaseSource(TestCases.AlterTableAddColumnsTestCases)]
        public void CanAlterTableAddColumns(INamePath tableName, CheckType? withCheck,
            IValuesFixture<IColumn> columns, string expectedSql)
        {
            // The real gains are realized here when we finally fluently build the alter statement.
            var actualSql = Alter.Table(tableName, withCheck).Add(columns.Values.ToArray()).ToString();

            Console.WriteLine("CanAlterTableAddColumns SQL: {0}", actualSql);

            Assert.That(actualSql, Is.EqualTo(expectedSql).IgnoreCase);
        }

        [Test]
        [TestCaseSource(TestCases.AlterTableDropColumnsTestCases)]
        public void CanAlterTableDropColumns(INamePath tableName, CheckType? withCheck,
            IValuesFixture<IColumn> columns, string expectedSql)
        {
            // The real gains are realized here when we finally fluently build the alter statement.
            var actualSql = Alter.Table(tableName, withCheck).Drop(columns.Values.ToArray()).ToString();

            Console.WriteLine("CanAlterTableDropColumns SQL: {0}", actualSql);

            Assert.That(actualSql, Is.EqualTo(expectedSql).IgnoreCase);
        }

        [Test]
        [TestCaseSource(TestCases.AlterTableAddConstraintsTestCases)]
        public void CanAlterTableAddConstraints(INamePath tableName, CheckType? withCheck,
            IValuesFixture<IConstraint> constraints, string expectedSql)
        {
            var actualSql = Alter.Table(tableName, withCheck).Add(constraints.Values.ToArray()).ToString();

            Console.Write("CanAlterTableAddConstraints SQL: {0}", actualSql);

            Assert.That(actualSql, Is.EqualTo(expectedSql).IgnoreCase);
        }

        [Test]
        [TestCaseSource(TestCases.AlterTableDropConstraintsTestCases)]
        public void CanAlterTableDropConstraints(INamePath tableName, CheckType? withCheck,
            IValuesFixture<IConstraint> constraints, string expectedSql)
        {
            var actualSql = Alter.Table(tableName, withCheck).Drop(constraints.Values.ToArray()).ToString();

            Console.Write("CanAlterTableDropConstraints SQL: {0}", actualSql);

            Assert.That(actualSql, Is.EqualTo(expectedSql).IgnoreCase);
        }
    }
}
