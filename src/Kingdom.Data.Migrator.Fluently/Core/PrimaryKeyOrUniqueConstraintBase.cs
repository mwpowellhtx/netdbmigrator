using System;
using System.Linq;

namespace Kingdom.Data
{
    /// <summary>
    /// Primary Key or Unique Index constraint.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public interface IPrimaryKeyOrUniqueConstraint<TParent>
        : IConstraint
            , IHasIndexColumns<IPrimaryKeyOrUniqueIndexColumn, TParent>
        where TParent : IPrimaryKeyOrUniqueConstraint<TParent>
    {
        /// <summary>
        /// Signals that the constraint is <see cref="TableIndexType.PrimaryKey"/>.
        /// </summary>
        TParent PrimaryKey { get; }

        /// <summary>
        /// Signals that the constraint is <see cref="TableIndexType.UniqueIndex"/>.
        /// </summary>
        TParent Unique { get; }

        /// <summary>
        /// Signals that the constraint is <see cref="ClusteredType.Clustered"/>.
        /// </summary>
        /// <see cref="ClusteredType"/>
        TParent Clustered { get; }

        /// <summary>
        /// Signals that the constraint is <see cref="ClusteredType.NonClustered"/>.
        /// </summary>
        /// <see cref="ClusteredType"/>
        TParent NonClustered { get; }
    }

    /// <summary>
    /// Primary key or unique index constraint base class.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public abstract class PrimaryKeyOrUniqueConstraintBase<TParent>
        : ConstraintBase<TParent>
            , IPrimaryKeyOrUniqueConstraint<TParent>
        where TParent : PrimaryKeyOrUniqueConstraintBase<TParent>
    {
        public TParent Clustered
        {
            get { return Set(ClusteredType.Clustered, () => new ClusteredConstraintAttribute()); }
        }

        public TParent NonClustered
        {
            get { return Set(ClusteredType.NonClustered, () => new ClusteredConstraintAttribute()); }
        }

        /// <summary>
        /// Returns the <see cref="ClusteredType"/> from the constraint.
        /// </summary>
        /// <returns></returns>
        protected ClusteredType? GetClusteredAttributeValue()
        {
            ClusteredType? result;
            return Attributes.TryFindColumnAttribute(out result, (ClusteredConstraintAttribute x) => x.Value)
                ? result
                : null;
        }

        public TParent PrimaryKey
        {
            get { return Set(TableIndexType.PrimaryKey, () => new TableIndexConstraintAttribute()); }
        }

        public TParent Unique
        {
            get { return Set(TableIndexType.UniqueIndex, () => new TableIndexConstraintAttribute()); }
        }

        private TableIndexType GetTableIndexAttributeValue()
        {
            TableIndexType result;

            if (!Attributes.TryFindColumnAttribute(out result, (TableIndexConstraintAttribute x) => x.Value))
            {
                throw this.ThrowNotSupportedException(() =>
                {
                    var delimited = CommaDelimited(this.GetValues<TableIndexType>().Select(x => x.ToString()));
                    return string.Format(@"Table index type is required: {0}", delimited);
                });
            }

            return result;
        }

        private IFluentCollection<IPrimaryKeyOrUniqueIndexColumn, TParent> _keyColumns;

        public IFluentCollection<IPrimaryKeyOrUniqueIndexColumn, TParent> KeyColumns
        {
            get { return _keyColumns; }
            set
            {
                _keyColumns
                    = value
                      ?? new FluentCollection<IPrimaryKeyOrUniqueIndexColumn, TParent>((TParent) this);
            }
        }

        /// <summary>
        /// Protected constructor.
        /// </summary>
        protected PrimaryKeyOrUniqueConstraintBase()
        {
            KeyColumns = null;
        }

        /// <summary>
        /// Returns the string representation of the <see cref="Clustered"/>
        /// or <see cref="NonClustered"/> signals.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetClusteredString()
        {
            var value = GetClusteredAttributeValue();

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (value)
            {
                case ClusteredType.Clustered:
                case ClusteredType.NonClustered:
                    return string.Format(@" {0}", value).ToUpper();
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Returns the string representation of the <see cref="PrimaryKey"/>
        /// or <see cref="Unique"/> signals.
        /// </summary>
        /// <returns></returns>
        /// <see cref="TableIndexType"/>
        protected string GetTableIndexString()
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (GetTableIndexAttributeValue())
            {
                case TableIndexType.PrimaryKey:
                    return "PRIMARY KEY";
                case TableIndexType.UniqueIndex:
                    return "UNIQUE";
            }

            throw ((object) null).ThrowNotSupportedException("Index type is required.");
        }

        public override string GetAddableString()
        {
            // TODO: TBD: some or all of this may be Sql Server specific; to be specialized at the appropriate level.
            var tableIndexString = GetTableIndexString().Trim();

            var clusteredString = GetClusteredString();

            // TODO: TBD: so I think this is close to working: just need to work out a couple of kinks in the outer edges of usage
            var columns = CommaDelimited(KeyColumns.Items.Select(x => x.GetPrimaryKeyOrUniqueString()));

            return string.Format("{0} {1} {2}{3} ({4})", SubjectName, Name, tableIndexString, clusteredString, columns);
        }
    }
}
