using System.Data;

namespace Kingdom.Data
{
    /// <summary>
    /// Sql Server column interface.
    /// </summary>
    public interface ISqlServerColumn : IColumn<SqlDbType>
    {
    }

    /// <summary>
    /// Represents a Sql Server column.
    /// </summary>
    public class SqlServerColumn : ColumnBase<SqlDbType>, ISqlServerColumn
    {
        private readonly SqlServerDataTypeRegistry _registry;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SqlServerColumn()
        {
            _registry = new SqlServerDataTypeRegistry();
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="columnName"></param>
        public SqlServerColumn(string columnName)
        {
            Name = NamePath.Create(columnName);
            _registry = new SqlServerDataTypeRegistry();
        }

        protected override string FormatTypeWithPrecisionScale(SqlDbType type, int? a = null, int? b = null)
        {
            return _registry.GetDbTypeString(type, a, b);
        }

        public override string GetAddableString()
        {
            // TODO: could add constraints here
            var nullableString = GetNullableString();
            return string.Format(@"{0} {1}{2}", Name, FormattedType, nullableString);
        }

        public override string GetDroppableString()
        {
            return Name.ToString();
        }

        public override string GetPrimaryKeyOrUniqueString()
        {
            var sortOrderString = GetSortOrderString();
            return string.Format("{0} {1}", Name, sortOrderString);
        }
    }
}
