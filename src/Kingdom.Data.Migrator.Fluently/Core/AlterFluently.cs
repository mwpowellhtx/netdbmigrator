using System.Collections.Generic;
using System.Linq;

namespace Kingdom.Data
{
    /// <summary>
    /// Represents a fluently rooted Alter statement.
    /// </summary>
    /// <typeparam name="TAlterTableFluently"></typeparam>
    public class FluentAlterRoot<TAlterTableFluently>
        : FluentRootBase
            , IAlterFluently<TAlterTableFluently>
        where TAlterTableFluently : class, IAlterTableFluently, new()
    {
        /// <summary>
        /// Returns a <see cref="TAlterTableFluently"/> instance given
        /// <paramref name="name"/> and <paramref name="withCheck"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="withCheck"></param>
        /// <returns></returns>
        public TAlterTableFluently Table(INamePath name, CheckType? withCheck = null)
        {
            return new TAlterTableFluently {TableName = name, WithCheck = withCheck};
        }
    }

    /// <summary>
    /// Represents fluent Alter Table base class everything Alter Table stems from this class.
    /// </summary>
    public abstract class FluentAlterTableBase
        : FluentRootBase
            , IAlterTableFluently
    {
        private AlterTableType? _type;

        private CheckType? _withCheck;

        /// <summary>
        /// Gets or sets the TableName.
        /// </summary>
        public INamePath TableName { get; set; }

        private readonly IList<ITableAddable> _addables;

        private readonly IList<ITableDroppable> _droppables;

        /// <summary>
        /// Protected constructor.
        /// </summary>
        protected FluentAlterTableBase()
        {
            _addables = new List<ITableAddable>();
            _droppables = new List<ITableDroppable>();
        }

        private AlterTableType Type
        {
            set { _type = _type ?? value; }
        }

        //TODO: with check should be an attribute? would certainly fit the pattern...
        public CheckType? WithCheck
        {
            get { return _withCheck; }
            set { _withCheck = value; }
        }

        /// <summary>
        /// Returns the string corresponding to the <see cref="_type"/> value.
        /// </summary>
        /// <returns></returns>
        /// <see cref="AlterTableType"/>
        protected string GetAlterTableTypeString()
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (_type)
            {
                case AlterTableType.Add:
                case AlterTableType.Drop:
                    return _type.Value.ToString().ToUpper();
            }

            throw this.ThrowNotSupportedException(() => string.Format(
                "Alter table type not supported: {0}",
                _type == null ? "null" : _type.Value.ToString()));
        }

        /// <summary>
        /// Returns the string corresponding to the <see cref="_withCheck"/> value.
        /// </summary>
        /// <returns></returns>
        /// <see cref="CheckType"/>
        protected string GetWithCheckTypeString()
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (_withCheck)
            {
                case CheckType.Check:
                case CheckType.NoCheck:
                    return _type == AlterTableType.Drop
                        ? string.Empty
                        : string.Format(@" with {0}", _withCheck.Value).ToUpper();
                case null:
                    return string.Empty;
            }

            throw this.ThrowNotSupportedException(() => string.Format(
                "Alter table with check not supported: {0}",
                _withCheck == null ? "null" : _withCheck.Value.ToString()));
        }

        /// <summary>
        /// Represents adding addable items to the Table. These should be consistent,
        /// such as all Columns, Constraints, and so on.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public IAlterTableAddFluently Add<T>(params T[] items)
            where T : class, ITableAddable
        {
            Type = AlterTableType.Add;
            foreach (var x in items) _addables.Add(x);
            return this;
        }

        /// <summary>
        /// Represents dropping droppable items from the Table. These should be consistent,
        /// such as all Columns, Constraints, and so on.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public IAlterTableDropFluently Drop<T>(params T[] items)
            where T : class, ITableDroppable
        {
            Type = AlterTableType.Drop;
            foreach (var x in items) _droppables.Add(x);
            return this;
        }

        private static IList<T> CheckSubjects<T>(IList<T> items)
            where T : ISubject
        {
            var subjects = items.Select(x => x.SubjectName).Distinct().ToList();

            if (!subjects.Any())
            {
                throw ((object) null).ThrowNotSupportedException(
                    "At least one subject is required.");
            }

            if (subjects.Count != 1)
            {
                throw ((object) null).ThrowNotSupportedException(() => string.Format(
                    "Alter subjects not homogenous: {0}", CommaDelimited(subjects)));
            }

            return items;
        }

        /// <summary>
        /// Returns the kind of Subject we are talking about. This does not apply to all circumstances.
        /// </summary>
        /// <returns></returns>
        protected string GetSubjectKind()
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (_type)
            {
                case AlterTableType.Add:
                    return string.Empty;
                case AlterTableType.Drop:
                    var kinds = CheckSubjects(_droppables).OfType<ISubject>().Select(x => x.SubjectName);
                    return string.Format(" {0}", kinds.Distinct().Single().Trim()).ToUpper();
            }

            throw ((object) null).ThrowNotSupportedException(() => string.Format(
                "Alter type not supported: {0}",
                _type == null ? "null" : _type.Value.ToString()));
        }

        /// <summary>
        /// Returns a collection of formatted Subject strings.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<string> GetSubjectStrings()
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (_type)
            {
                case AlterTableType.Add:
                    return CheckSubjects(_addables).Select(x => x.GetAddableString());
                case AlterTableType.Drop:
                    return CheckSubjects(_droppables).Select(x => x.GetDroppableString());
            }

            throw this.ThrowNotSupportedException(() => string.Format(
                "Alter type not supported: {0}",
                _type == null ? "null" : _type.Value.ToString()));
        }

        /// <summary>
        /// Returns the Sql string built and ready to go.
        /// </summary>
        /// <returns></returns>
        protected abstract string BuildSql();

        public override string ToString()
        {
            return BuildSql();
        }
    }
}
