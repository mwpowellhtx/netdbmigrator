//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Kingdom.Data
//{
//    /// <summary>
//    /// Represents the Alter Table helper.
//    /// </summary>
//    public interface IAlterTableHelper : IAlterTableHelperBase
//    {
//        /// <summary>
//        /// Sets the Table <paramref name="namePath"/>.
//        /// </summary>
//        /// <param name="namePath"></param>
//        /// <returns></returns>
//        IAlterTableHelper TableName(INamePath namePath);
//    }

//    /// <summary>
//    /// Represents the Alter Table command specific to the Sql Server database provider.
//    /// </summary>
//    public class AlterTableHelper : AlterTableHelperBase<AlterTableType>, IAlterTableHelper
//    {
//        private INamePath _tableNamePath;

//        private AlterTableType? _type;

//        private IList<object> _subjects;

//        private static string GetTypeString(AlterTableType? type)
//        {
//            Func<AlterTableType?, string> format = x => type == null ? "(null)" : type.Value.ToString();

//            // ReSharper disable once SwitchStatementMissingSomeCases
//            switch (type)
//            {
//                case AlterTableType.Add:
//                case AlterTableType.Drop:
//                    return format(type);
//            }

//            throw ThrowNotSupportedException(type, format);
//        }

//        /// <summary>
//        /// Default constructor.
//        /// </summary>
//        public AlterTableHelper()
//        {
//            _type = null;
//            _subjects = new List<object>();
//        }

//        public IAlterTableHelper TableName(INamePath namePath)
//        {
//            _tableNamePath = namePath;
//            return this;
//        }

//        /// <summary>
//        /// Adds a range of <paramref name="addables"/> to the helper.
//        /// </summary>
//        /// <param name="addables"></param>
//        /// <returns></returns>
//        public IAlterTableHelper Add(params ITableAddable[] addables)
//        {
//            _type = AlterTableType.Add;
//            return HelperAction<AlterTableHelper>(x => x._subjects = addables.ToList<object>());
//        }

//        /// <summary>
//        /// Adds a range of <paramref name="droppables"/> to the helper.
//        /// </summary>
//        /// <param name="droppables"></param>
//        /// <returns></returns>
//        public IAlterTableHelper Drop(params ITableDroppable[] droppables)
//        {
//            _type = AlterTableType.Drop;
//            return HelperAction<AlterTableHelper>(x => x._subjects = droppables.ToList<object>());
//        }

//        protected override string FormatAction(AlterTableType action)
//        {
//            throw ThrowNotSupportedException(action);
//        }

//        /// <summary>
//        /// Returns the subject type of the thing being operated on by the action type.
//        /// </summary>
//        /// <returns></returns>
//        private string GetSubjectTypeString()
//        {
//            if (_subjects.All(x => x is IColumn))
//                return "COLUMN";

//            else if (_subjects.All(x => x is IConstraint))
//                return "CONSTRAINT";

//            throw ThrowNotSupportedException();
//        }

//        protected override string Build()
//        {
//            Func<AlterTableType?, string> format = x => x == null ? "(null)" : x.Value.ToString();

//            // ReSharper disable once SwitchStatementMissingSomeCases
//            switch (_type)
//            {
//                case AlterTableType.Add:
//                case AlterTableType.Drop:
//                    return string.Format("ALTER TABLE {0} {1} {2}", _tableNamePath, GetTypeString(_type));
//            }

//            throw ThrowNotSupportedException(_type, format);
//        }
//    }

//}
