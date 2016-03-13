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
        /// Gets the Clustered type.
        /// </summary>
        /// <see cref="ClusteredType"/>
        ClusteredType? Clustered { get; }

        /// <summary>
        /// Gets the IndexType.
        /// </summary>
        TableIndexType IndexType { get; }
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
        public ClusteredType? Clustered
        {
            get
            {
                ClusteredType? result;
                return Attributes.TryFindColumnAttribute(out result, (ClusteredConstraintAttribute x) => x.Value)
                    ? result
                    : null;
            }
        }

        /// <summary>
        /// Gets the IndexType.
        /// </summary>
        public TableIndexType IndexType
        {
            get
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
        /// Returns the string representation of the <see cref="Clustered"/> attribute.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetClusteredString()
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (Clustered)
            {
                case ClusteredType.Clustered:
                case ClusteredType.NonClustered:
                    return string.Format(@" {0}", Clustered).ToUpper();
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Returns the string representation of the <see cref="IndexType"/>.
        /// </summary>
        /// <returns></returns>
        /// <see cref="TableIndexType"/>
        protected string GetTableIndexString()
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (IndexType)
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
