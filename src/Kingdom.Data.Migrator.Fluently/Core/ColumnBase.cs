namespace Kingdom.Data
{
    /// <summary>
    /// Represents a Primary Key or Unique Index Column.
    /// </summary>
    public interface IPrimaryKeyOrUniqueIndexColumn
    {
        /// <summary>
        /// Gets the Order.
        /// </summary>
        /// <see cref="SortOrder"/>
        SortOrder? Order { get; }

        /// <summary>
        /// Returns the string representing the primary key or unique index column.
        /// </summary>
        /// <returns></returns>
        string GetPrimaryKeyOrUniqueString();
    }

    /// <summary>
    /// Represents the Column concept.
    /// </summary>
    public interface IColumn
        : ISubject
            , ITableAddable
            , ITableDroppable
            , IPrimaryKeyOrUniqueIndexColumn
            , IHasDataAttributes<IColumnAttribute>
            , IFluentCollection<IColumnAttribute, IColumn>
    {
        /// <summary>
        /// Gets or sets the Column Name.
        /// </summary>
        INamePath Name { get; set; }

        /// <summary>
        /// Gets the FormattedType.
        /// </summary>
        /// <see cref="IDataTypeRegistry"/>
        string FormattedType { get; }

        /// <summary>
        /// Gets whether CanBeNull.
        /// </summary>
        bool? CanBeNull { get; }
    }

    /// <summary>
    /// Represents the Column concept.
    /// </summary>
    /// <typeparam name="TDbType"></typeparam>
    public interface IColumn<TDbType> : IColumn
    {
        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        TDbType Type { get; set; }
    }

    /// <summary>
    /// Represents the Column concept.
    /// </summary>
    public abstract class ColumnBase
        : DataBase<IColumnAttribute>
            , IColumn
    {
        /// <summary>
        /// Gets the SubjectName: &quot;COLUMN&quot;
        /// </summary>
        public string SubjectName
        {
            get { return @"COLUMN"; }
        }

        //TODO: may put in some formatted, validation, etc...
        /// <summary>
        /// Gets or sets the Column Name.
        /// </summary>
        public INamePath Name { get; set; }

        /// <summary>
        /// Gets the FormattedType.
        /// </summary>
        public abstract string FormattedType { get; }

        /// <summary>
        /// Gets whether CanBeNull.
        /// </summary>
        public bool? CanBeNull
        {
            get { return GetCanBeNull(); }
        }

        private bool? GetCanBeNull()
        {
            bool result;
            return TryFindColumnAttribute(Attributes, out result,
                (NullableColumnAttribute x) => x.Value)
                ? result
                : (bool?)null;
        }

        /// <summary>
        /// Gets the Order.
        /// </summary>
        public SortOrder? Order
        {
            get { return GetSortOrder(); }
        }

        private SortOrder? GetSortOrder()
        {
            SortOrder result;
            return TryFindColumnAttribute(Attributes, out result,
                (SortOrderColumnAttribute x) => x.Value)
                ? result
                : (SortOrder?) null;
        }

        /// <summary>
        /// Returns the string representation of the <see cref="Order"/>.
        /// </summary>
        /// <returns></returns>
        /// <see cref="SortOrder"/>
        protected string GetSortOrderString()
        {
            var order = Order;

            const SortOrder ascending = SortOrder.Ascending;
            const SortOrder descending = SortOrder.Descending;

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (Order)
            {
                case SortOrder.Ascending:
                    return ascending.ToString().Substring(0, 3).ToUpper();
                case SortOrder.Descending:
                    return descending.ToString().Substring(0, 4).ToUpper();
            }

            throw this.ThrowNotSupportedException(() => string.Format(
                @"Sort order is not supported: {0}",
                order == null ? "null" : order.Value.ToString()));
        }

        public IColumn Add(IColumnAttribute item, params IColumnAttribute[] items)
        {
            Attributes.Add(item);
            foreach (var x in items) Attributes.Add(x);
            return this;
        }

        /// <summary>
        /// Returns the Nullable string.
        /// </summary>
        protected string GetNullableString()
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (CanBeNull)
            {
                case true:
                    return @" NULL";
                case false:
                    return @" NOT NULL";
                default:
                    return string.Empty;
            }
        }

        public abstract string GetAddableString();

        public abstract string GetDroppableString();

        public abstract string GetPrimaryKeyOrUniqueString();
    }

    /// <summary>
    /// Represents the Column concept.
    /// </summary>
    /// <typeparam name="TDbType"></typeparam>
    public abstract class ColumnBase<TDbType> : ColumnBase, IColumn<TDbType>
    {
        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        public TDbType Type { get; set; }

        /// <summary>
        /// Gets the FormattedType.
        /// </summary>
        public sealed override string FormattedType
        {
            get { return GetFormattedType(Type); }
        }

        private string GetFormattedType(TDbType type)
        {
            int precision;
            int scale;

            // These type conversions are intentional.
            var foundPrecision = TryFindColumnAttribute(Attributes, out precision,
                (PrecisionColumnAttribute x) => x.Value);

            var foundScale = TryFindColumnAttribute(Attributes, out scale,
                (ScaleColumnAttribute x) => x.Value);

            if (foundPrecision && !foundScale)
                return FormatTypeWithPrecisionScale(type, precision);

            return foundPrecision
                ? FormatTypeWithPrecisionScale(type, precision, scale)
                : FormatTypeWithPrecisionScale(type);
        }

        /// <summary>
        /// Returns the <see cref="FormattedType"/> for the column considering precision
        /// and scale, as informed by <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected abstract string FormatTypeWithPrecisionScale(TDbType type, int? a = null, int? b = null);
    }
}
