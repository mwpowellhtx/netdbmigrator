using System.Data;

namespace Kingdom.Data
{
   
    /// <summary>
    /// Represtns a Data Type registry for Sql Server.
    /// </summary>
    public interface ISqlServerDataTypeRegistry : IDataTypeRegistry
    {
        /// <summary>
        /// Returns the string representation of the database <paramref name="type"/> with
        /// optional <paramref name="a"/> and <paramref name="b"/> parameters.
        /// See <see cref="IDataTypeRegistry.GetDbTypeString(DbType,int?,int?)"/>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <see cref="IDataTypeRegistry.GetDbTypeString(DbType,int?,int?)"/>
        string GetDbTypeString(SqlDbType type, int? a = null, int? b = null);
    }

    /// <summary>
    /// Represents the Alter Table command specific to the Sql Server database provider.
    /// </summary>
    public class SqlServerDataTypeRegistry : DataTypeRegistryBase, ISqlServerDataTypeRegistry
    {
        public override string GetDbTypeString(DbType type, int? a = null, int? b = null)
        {
            var lengthStr = GetDataTypeLength(a ?? b);

            /* TODO: may want a DECIMAL, NUMERIC one; which takes two "lengths"; p (precision) and s (scale)
             * decimal / numeric: https://msdn.microsoft.com/en-us/library/ms187746.aspx
             * float / real: https://msdn.microsoft.com/en-us/library/ms173773.aspx
             * mapping clr parameter data: https://msdn.microsoft.com/en-us/library/ms131092.aspx
             */

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (type)
            {
                case DbType.AnsiStringFixedLength:
                    return NamePath.Create("CHAR").ToString() + lengthStr;

                case DbType.StringFixedLength:
                    return NamePath.Create("NCHAR").ToString() + lengthStr;

                case DbType.AnsiString:
                    return NamePath.Create("VARCHAR").ToString() + lengthStr;

                case DbType.String:
                    return NamePath.Create("NVARCHAR").ToString() + lengthStr;

                case DbType.Boolean:
                    return NamePath.Create("BIT").ToString();

                case DbType.Byte:
                    return NamePath.Create("TINYINT").ToString();

                case DbType.Int16:
                    return NamePath.Create("SMALLINT").ToString();

                case DbType.Int32:
                    return NamePath.Create("INT").ToString();

                case DbType.Int64:
                    return NamePath.Create("BIGINT").ToString();

                case DbType.Single:
                    return NamePath.Create("REAL").ToString();

                case DbType.Double:
                    return NamePath.Create("FLOAT").ToString() + lengthStr;

                case DbType.Date:
                    return NamePath.Create("DATE").ToString();

                case DbType.DateTime:
                    return NamePath.Create("DATETIME").ToString();

                case DbType.DateTime2:
                    return NamePath.Create("DATETIME2").ToString() + lengthStr;

                case DbType.DateTimeOffset:
                    return NamePath.Create("DATETIMEOFFSET").ToString() + lengthStr;

                case DbType.Time:
                    return NamePath.Create("TIME").ToString() + lengthStr;

                case DbType.Guid:
                    return NamePath.Create("UNIQUEIDENTIFIER").ToString();

                case DbType.Binary:
                    return NamePath.Create("BINARY").ToString() + lengthStr;
            }

            return base.GetDbTypeString(type, a, b);
        }

        /// <summary>
        /// Returns the string representation of the database <paramref name="type"/> with
        /// optional <paramref name="a"/> and <paramref name="b"/> parameters. See
        /// <see cref="IDataTypeRegistry.GetDbTypeString(DbType,int?,int?)"/>. If you are
        /// working with Sql Server, then this is the preferred way to format database types.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <see cref="IDataTypeRegistry.GetDbTypeString(DbType,int?,int?)"/>
        public string GetDbTypeString(SqlDbType type, int? a = null, int? b = null)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (type)
            {
                case SqlDbType.Char:
                    return GetDbTypeString(DbType.AnsiStringFixedLength, a, b);

                case SqlDbType.NChar:
                    return GetDbTypeString(DbType.StringFixedLength, a, b);

                case SqlDbType.VarChar:
                    return GetDbTypeString(DbType.AnsiString, a, b);

                case SqlDbType.NVarChar:
                    return GetDbTypeString(DbType.String, a, b);

                case SqlDbType.Bit:
                    return GetDbTypeString(DbType.Boolean);

                case SqlDbType.TinyInt:
                    return GetDbTypeString(DbType.Byte);

                case SqlDbType.SmallInt:
                    return GetDbTypeString(DbType.Int16);

                case SqlDbType.Int:
                    return GetDbTypeString(DbType.Int32);
                
                case SqlDbType.BigInt:
                    return GetDbTypeString(DbType.Int64);

                case SqlDbType.Real:
                    return GetDbTypeString(DbType.Single);

                case SqlDbType.Float:
                    return GetDbTypeString(DbType.Double, a, b);

                case SqlDbType.Date:
                    return GetDbTypeString(DbType.Date);

                case SqlDbType.DateTime:
                    return GetDbTypeString(DbType.DateTime);

                case SqlDbType.DateTime2:
                    return GetDbTypeString(DbType.DateTime2, a, b);

                case SqlDbType.DateTimeOffset:
                    return GetDbTypeString(DbType.DateTimeOffset, a, b);

                case SqlDbType.Time:
                    return GetDbTypeString(DbType.Time, a, b);

                case SqlDbType.UniqueIdentifier:
                    return GetDbTypeString(DbType.Guid, a, b);

                case SqlDbType.Binary:
                    return GetDbTypeString(DbType.Binary, a, b);

                case SqlDbType.VarBinary:
                    // This one is a special case apart from the base class.
                    var lengthStr = GetDataTypeLength(a ?? b);
                    return NamePath.Create("VARBINARY").ToString() + lengthStr;
            }

            throw ThrowNotSupportedException(type, a, b);
        }

    }
}
