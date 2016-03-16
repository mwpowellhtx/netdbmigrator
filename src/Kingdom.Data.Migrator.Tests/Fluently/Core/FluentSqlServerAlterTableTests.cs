using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Kingdom.Data
{
    using NUnit.Framework;

    public class FluentSqlServerAlterTableTests
        : FluentlyAlterTestFixtureBase<SqlServerFluentAlterTable>
    {
        private static TParent CreateColumn<TParent>(string columnName)
            where TParent : class, IColumn<TParent>, new()
        {
            return new TParent {Name = NamePath.Create(columnName)};
        }

        private static TParent CreateColumn<TParent>(string columnName, SqlDbType type)
            where TParent : class, IColumn<SqlDbType, TParent>, new()
        {
            return new TParent {Name = NamePath.Create(columnName), Type = type};
        }

        private static SqlServerColumn CreateSqlServerColumn(string columnName)
        {
            return CreateColumn<SqlServerColumn>(columnName);
        }

        private static SqlServerColumn CreateSqlServerColumn(string columnName, SqlDbType type)
        {
            return CreateColumn<SqlServerColumn>(columnName, type);
        }

        private static TConstraint CreateConstraint<TConstraint>(
            string constraintName, Action<TConstraint> initialize = null)
            where TConstraint : class, IConstraint, new()
        {
            initialize = initialize ?? (c => { });
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

            yield return new TestCaseData(fooNamePath(), (CheckType?) null, BuildEnumeration<IColumn>(
                CreateSqlServerColumn(intName, intType).Attributes
                    .Add(new NullableColumnAttribute()),
                CreateSqlServerColumn(bufferName, varBinaryType).Attributes
                    .Add(new PrecisionColumnAttribute(16))
                , CreateSqlServerColumn(floatName, floatType).Attributes
                    .Add(new PrecisionColumnAttribute(42), new ScaleColumnAttribute(3))
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[foo] ADD [myInt] [INT] NOT NULL"
                  + ", [myBuffer] [VARBINARY](16), [myFloat] [FLOAT](42);"
                );

            yield return new TestCaseData(barNamePath(), CheckType.Check, BuildEnumeration<IColumn>(
                CreateSqlServerColumn(intName, intType).Attributes
                    .Add(new NullableColumnAttribute())
                , CreateSqlServerColumn(bufferName, varBinaryType).Attributes
                    .Add(new NullableColumnAttribute(true)
                        , new PrecisionColumnAttribute(16))
                , CreateSqlServerColumn(floatName, floatType).Attributes
                    .Add(new PrecisionColumnAttribute(42), new ScaleColumnAttribute(3))
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[bar] WITH CHECK ADD [myInt] [INT] NOT NULL"
                  + ", [myBuffer] [VARBINARY](16) NULL, [myFloat] [FLOAT](42);"
                );

            yield return new TestCaseData(fizNamePath(), CheckType.NoCheck, BuildEnumeration<IColumn>(
                CreateSqlServerColumn(intName, intType).Attributes
                    .Add(new NullableColumnAttribute())
                , CreateSqlServerColumn(bufferName, varBinaryType).Attributes
                    .Add(new PrecisionColumnAttribute(16))
                , CreateSqlServerColumn(floatName, floatType)
                    .Attributes.Add(new PrecisionColumnAttribute(42), new ScaleColumnAttribute(3))
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] WITH NOCHECK ADD [myInt] [INT] NOT NULL"
                  + ", [myBuffer] [VARBINARY](16), [myFloat] [FLOAT](42);"
                );

            yield return new TestCaseData(fizNamePath(), CheckType.NoCheck, BuildEnumeration<IColumn>(
                CreateSqlServerColumn(intName, intType).Attributes
                    .Add(new IdentityColumnAttribute(), new NullableColumnAttribute())
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] WITH NOCHECK ADD [myInt] [INT] IDENTITY NOT NULL;"
                );

            yield return new TestCaseData(fizNamePath(), CheckType.NoCheck, BuildEnumeration<IColumn>(
                CreateSqlServerColumn(intName, intType).Attributes
                    .Add(new SeededIdentityColumnAttribute(1, 2), new NullableColumnAttribute())
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] WITH NOCHECK ADD [myInt] [INT] IDENTITY(1, 2) NOT NULL;"
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

            yield return new TestCaseData(fooNamePath(), (CheckType?) null, BuildEnumeration<IColumn>(
                CreateSqlServerColumn(intName), CreateSqlServerColumn(bufferName)
                , CreateSqlServerColumn(floatName)).ToValuesFixture()
                , "ALTER TABLE [dbo].[foo] DROP COLUMN [myInt], [myBuffer], [myFloat];"
                );

            yield return new TestCaseData(barNamePath(), CheckType.Check, BuildEnumeration<IColumn>(
                CreateSqlServerColumn(intName), CreateSqlServerColumn(bufferName)
                , CreateSqlServerColumn(floatName)).ToValuesFixture()
                , "ALTER TABLE [dbo].[bar] DROP COLUMN [myInt], [myBuffer], [myFloat];"
                );

            yield return new TestCaseData(fizNamePath(), CheckType.NoCheck, BuildEnumeration<IColumn>(
                CreateSqlServerColumn(intName), CreateSqlServerColumn(bufferName)
                , CreateSqlServerColumn(floatName)).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] DROP COLUMN [myInt], [myBuffer], [myFloat];"
                );

            // All or nothing: if some of them have IfExists(), then will no specify.
            yield return new TestCaseData(fizNamePath(), CheckType.NoCheck, BuildEnumeration<IColumn>(
                CreateSqlServerColumn(intName).IfExists(), CreateSqlServerColumn(bufferName)
                , CreateSqlServerColumn(floatName)).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] DROP COLUMN [myInt], [myBuffer], [myFloat];"
                );

            // All or nothing specify IfExists().
            yield return new TestCaseData(fizNamePath(), CheckType.NoCheck, BuildEnumeration<IColumn>(
                CreateSqlServerColumn(intName).IfExists(), CreateSqlServerColumn(bufferName).IfExists()
                , CreateSqlServerColumn(floatName).IfExists()).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] DROP COLUMN IF EXISTS [myInt], [myBuffer], [myFloat];"
                );
        }

        private static IEnumerable<TestCaseData> GetAlterTableAddConstraintsTestCases()
        {
            // There's only so much we can do to help with the verbosity.
            Func<INamePath> fooNamePath = () => NamePath.Create("dbo", "foo");
            Func<INamePath> fizNamePath = () => NamePath.Create("dbo", "fiz");

            const string barPrimaryKeyName = "PK_foo_bar";
            const string bazPrimaryKeyName = "PK_fiz_baz";
            const string effDefaultName = "DF_foo_eff";
            const string fuzDefaultName = "CK_foo_fuz";
            const string fizFooForeignKeyName = "FK_fiz_foo";

            const string intName = "myInt";
            const string floatName = "myFloat";

            const int floatValue = 42;

            const SortOrder ascending = SortOrder.Ascending;
            const SortOrder descending = SortOrder.Descending;

            yield return new TestCaseData(fooNamePath(), (CheckType?) null, BuildEnumeration<IConstraint>(
                CreateConstraint(barPrimaryKeyName, (SqlServerPrimaryKeyOrUniqueConstraint c) =>
                    c.PrimaryKey.Clustered.KeyColumns.Add(CreateSqlServerColumn(intName)
                        .Attributes.Add(new SortOrderColumnAttribute(ascending))))
                , CreateConstraint(effDefaultName, (SqlServerDefaultConstraint c) =>
                    c.ConstantExpression(() => floatValue.ToString())
                        .For(CreateSqlServerColumn(floatName))).WithValues
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[foo] ADD CONSTRAINT [PK_foo_bar] PRIMARY KEY CLUSTERED ([myInt] ASC)"
                  + ", CONSTRAINT [DF_foo_eff] DEFAULT 42 FOR [myFloat] WITH VALUES;"
                );

            yield return new TestCaseData(fizNamePath(), (CheckType?) null, BuildEnumeration<IConstraint>(
                CreateConstraint(effDefaultName, (SqlServerDefaultConstraint c) =>
                    c.For(CreateSqlServerColumn(floatName))).WithValues
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] ADD CONSTRAINT [DF_foo_eff] DEFAULT NULL FOR [myFloat] WITH VALUES;"
                );

            yield return new TestCaseData(fizNamePath(), (CheckType?) null, BuildEnumeration<IConstraint>(
                CreateConstraint(bazPrimaryKeyName, (SqlServerPrimaryKeyOrUniqueConstraint c) =>
                    c.KeyColumns.Add(CreateSqlServerColumn(floatName)
                        .Attributes.Add(new SortOrderColumnAttribute(descending)))).Unique.NonClustered
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] ADD CONSTRAINT [PK_fiz_baz] UNIQUE NONCLUSTERED ([myFloat] DESC);"
                );

            yield return new TestCaseData(fizNamePath(), (CheckType?) null, BuildEnumeration<IConstraint>(
                CreateConstraint(fuzDefaultName, (SqlServerCheckConstraint c) =>
                    c.NotForReplication.LogicalExpression(() => string.Format(@"[{0}]>0", floatName)))
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] ADD CONSTRAINT [CK_foo_fuz] CHECK NOT FOR REPLICATION ([myFloat]>0);"
                );

            yield return new TestCaseData(fizNamePath(), (CheckType?) null, BuildEnumeration<IConstraint>(
                CreateConstraint(fuzDefaultName, (SqlServerCheckConstraint c) =>
                    c.LogicalExpression(() => string.Format(@"[{0}]>0", floatName)))
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] ADD CONSTRAINT [CK_foo_fuz] CHECK ([myFloat]>0);"
                );

            yield return new TestCaseData(fizNamePath(), (CheckType?) null, BuildEnumeration<IConstraint>(
                CreateConstraint(fizFooForeignKeyName, (SqlServerForeignKeyConstraint c) =>
                    c.Columns.Add(ForeignKeyColumns.MyInt, ForeignKeyColumns.MyFloat)
                        .References.Table(fooNamePath())
                        .Columns.Add(ReferenceColumns.MyInt, ReferenceColumns.MyFloat))
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] ADD CONSTRAINT [FK_fiz_foo]"
                  + " FOREIGN KEY ([myInt], [myFloat]) REFERENCES [dbo].[foo] ([myInt], [myFloat]);"
                );

            yield return new TestCaseData(fizNamePath(), (CheckType?) null, BuildEnumeration<IConstraint>(
                CreateConstraint(fizFooForeignKeyName, (SqlServerForeignKeyConstraint c) =>
                    c.Columns.Add(ForeignKeyColumns.MyInt, ForeignKeyColumns.MyFloat)
                        .References.Table(fooNamePath())
                        .Columns.Add(ReferenceColumns.MyInt, ReferenceColumns.MyFloat)
                        .OnDelete(ForeignKeyAction.SetDefault).OnUpdate(ForeignKeyAction.Cascade))
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] ADD CONSTRAINT [FK_fiz_foo]"
                  + " FOREIGN KEY ([myInt], [myFloat]) REFERENCES [dbo].[foo] ([myInt], [myFloat])"
                  + " ON DELETE SET DEFAULT ON UPDATE CASCADE;"
                );
        }

        /// <summary>
        /// Columns useful during the process of building out a fluent Foreign Key statement.
        /// </summary>
        /// <see cref="IForeignKeyColumn"/>
        private static class ForeignKeyColumns
        {
            internal static readonly IForeignKeyColumn MyInt = new SqlServerColumn("myInt");
            internal static readonly IForeignKeyColumn MyFloat = new SqlServerColumn("myFloat");
        }

        /// <summary>
        /// Columns useful during the process of building out a fluent Foreign Key statement.
        /// </summary>
        /// <see cref="IReferenceColumn"/>
        private static class ReferenceColumns
        {
            internal static readonly IReferenceColumn MyInt = new SqlServerColumn("myInt");
            internal static readonly IReferenceColumn MyFloat = new SqlServerColumn("myFloat");
        }

        private static IEnumerable<TestCaseData> GetAlterTableDropConstraintsTestCases()
        {
            // There's only so much we can do to help with the verbosity.
            Func<INamePath> fooNamePath = () => NamePath.Create("dbo", "foo");
            Func<INamePath> fizNamePath = () => NamePath.Create("dbo", "fiz");

            const string barPrimaryKeyName = "PK_foo_bar";
            const string bazPrimaryKeyName = "PK_fiz_baz";
            const string oxeDefaultName = "DF_car_oxe";
            const string picDefaultName = "DF_car_pic";

            yield return new TestCaseData(fooNamePath(), (CheckType?) null, BuildEnumeration<IConstraint>(
                CreateConstraint<SqlServerPrimaryKeyOrUniqueConstraint>(barPrimaryKeyName)
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[foo] DROP CONSTRAINT [PK_foo_bar];"
                );

            yield return new TestCaseData(fooNamePath(), (CheckType?) null, BuildEnumeration<IConstraint>(
                CreateConstraint<SqlServerDefaultConstraint>(oxeDefaultName)
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[foo] DROP CONSTRAINT [DF_car_oxe];"
                );

            yield return new TestCaseData(fizNamePath(), (CheckType?) null, BuildEnumeration<IConstraint>(
                CreateConstraint<SqlServerPrimaryKeyOrUniqueConstraint>(bazPrimaryKeyName)
                , CreateConstraint<SqlServerDefaultConstraint>(oxeDefaultName)
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] DROP CONSTRAINT [PK_fiz_baz], [DF_car_oxe];"
                );

            // All or nothing if only some are specified IfExists() then will not render.
            yield return new TestCaseData(fizNamePath(), (CheckType?) null, BuildEnumeration<IConstraint>(
                CreateConstraint<SqlServerPrimaryKeyOrUniqueConstraint>(bazPrimaryKeyName).IfExists()
                , CreateConstraint<SqlServerDefaultConstraint>(oxeDefaultName)
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] DROP CONSTRAINT [PK_fiz_baz], [DF_car_oxe];"
                );

            // All or nothing if all have IfExists() then will render.
            yield return new TestCaseData(fizNamePath(), (CheckType?) null, BuildEnumeration<IConstraint>(
                CreateConstraint<SqlServerPrimaryKeyOrUniqueConstraint>(bazPrimaryKeyName).IfExists()
                , CreateConstraint<SqlServerDefaultConstraint>(oxeDefaultName).IfExists()
                , CreateConstraint<SqlServerDefaultConstraint>(picDefaultName).IfExists()
                ).ToValuesFixture()
                , "ALTER TABLE [dbo].[fiz] DROP CONSTRAINT IF EXISTS [PK_fiz_baz], [DF_car_oxe], [DF_car_pic];"
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
        public void CanAlterTableAddColumns(INamePath tableName, CheckType? checkType,
            IValuesFixture<IColumn> columns, string expectedSql)
        {
            // The real gains are realized here when we finally fluently build the alter statement.
            var fluently = With(Alter.Table(tableName), checkType);

            var actualSql = fluently.Add(columns.Values.ToArray()).ToString();

            Console.WriteLine("CanAlterTableAddColumns SQL: {0}", actualSql);

            Assert.That(actualSql, Is.EqualTo(expectedSql).IgnoreCase);
        }

        [Test]
        [TestCaseSource(TestCases.AlterTableDropColumnsTestCases)]
        public void CanAlterTableDropColumns(INamePath tableName, CheckType? checkType,
            IValuesFixture<IColumn> columns, string expectedSql)
        {
            // The real gains are realized here when we finally fluently build the alter statement.
            var fluently = With(Alter.Table(tableName), checkType);

            var actualSql = fluently.Drop(columns.Values.ToArray()).ToString();

            Console.WriteLine("CanAlterTableDropColumns SQL: {0}", actualSql);

            Assert.That(actualSql, Is.EqualTo(expectedSql).IgnoreCase);
        }

        [Test]
        [TestCaseSource(TestCases.AlterTableAddConstraintsTestCases)]
        public void CanAlterTableAddConstraints(INamePath tableName, CheckType? checkType,
            IValuesFixture<IConstraint> constraints, string expectedSql)
        {
            var fluently = With(Alter.Table(tableName), checkType);

            var actualSql = fluently.Add(constraints.Values.ToArray()).ToString();

            Console.Write("CanAlterTableAddConstraints SQL: {0}", actualSql);

            Assert.That(actualSql, Is.EqualTo(expectedSql).IgnoreCase);
        }

        [Test]
        [TestCaseSource(TestCases.AlterTableDropConstraintsTestCases)]
        public void CanAlterTableDropConstraints(INamePath tableName, CheckType? checkType,
            IValuesFixture<IConstraint> constraints, string expectedSql)
        {
            var fluently = With(Alter.Table(tableName), checkType);

            var actualSql = fluently.Drop(constraints.Values.ToArray()).ToString();

            Console.Write("CanAlterTableDropConstraints SQL: {0}", actualSql);

            Assert.That(actualSql, Is.EqualTo(expectedSql).IgnoreCase);
        }
    }
}
