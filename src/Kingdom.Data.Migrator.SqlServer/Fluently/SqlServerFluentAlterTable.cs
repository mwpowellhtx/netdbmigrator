namespace Kingdom.Data
{
    /// <summary>
    /// Provides fluent Alter Table helpers for Sql Server.
    /// </summary>
    public class SqlServerFluentAlterTable
        : FluentAlterTableBase<SqlServerFluentAlterTable>
    {
        /// <summary>
        /// Builds the Sql string for Sql Server Alter Table.
        /// </summary>
        /// <returns></returns>
        protected override string BuildSql()
        {
            var withCheckString = GetWithCheckTypeString();
            var alterTableTypeString = GetAlterTableTypeString();

            var subjectKind = GetSubjectKind();
            var ifExists = GetIfExistsString();

            var subjects = GetSubjectStrings();
            var delimitedSubjects = CommaDelimited(subjects);

            // TODO: TBD: this pattern works for simple Add/Drop elements; may need to abstract this out a bit for more complex Alter <What/> Scenarios.
            var sql = string.Format(@"ALTER TABLE {0}{1} {2}{3}{4} {5};", TableName,
                withCheckString, alterTableTypeString, subjectKind, ifExists, delimitedSubjects);

            return sql;
        }
    }
}
