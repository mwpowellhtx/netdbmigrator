namespace Kingdom.Data
{
    /// <summary>
    /// Provides fluent Alter Table helpers for Sql Server.
    /// </summary>
    public class SqlServerFluentAlterTable : FluentAlterTableBase
    {
        /// <summary>
        /// Builds the Sql string for Sql Server Alter Table.
        /// </summary>
        /// <returns></returns>
        protected override string BuildSql()
        {
            var withCheckString = GetWithCheckTypeString();
            var alterTableTypeString = GetAlterTableTypeString();

            var subjects = GetSubjectStrings();
            var delimitedSubjects = CommaDelimited(subjects);

            var sql = string.Format(@"ALTER TABLE {0}{1} {2} {3};", TableName,
                withCheckString, alterTableTypeString, delimitedSubjects);

            return sql;
        }
    }
}
