using System.Data;

namespace Kingdom.Data
{
    /// <summary>
    /// Represents a Sql Server column.
    /// </summary>
    public class SqlServerColumn : ColumnBase<SqlDbType>
    {
        private readonly SqlServerDataTypeRegistry _registry;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SqlServerColumn()
        {
            _registry = new SqlServerDataTypeRegistry();
        }

        protected override string FormatTypeWithPrecisionScale(SqlDbType type, int? a = null, int? b = null)
        {
            return _registry.GetDbTypeString(type, a, b);
        }

        public override string GetAddableString()
        {
            // TODO: could add constraints here

            return string.Format(@"{0} {1}{2}", Name, FormattedType, FormattedNullable);
        }

        public override string GetDroppableString()
        {
            return Name.ToString();
        }
    }
}
