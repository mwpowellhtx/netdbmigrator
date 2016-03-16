namespace Kingdom.Data
{
    /// <summary>
    /// Column interface.
    /// </summary>
    public interface IColumn : ITableAddable, ITableDroppable
    {
        /// <summary>
        /// Gets or sets the Column Name.
        /// </summary>
        INamePath Name { get; set; }
    }

    /// <summary>
    /// Represents the bits that represent a Column to the Default Constraint.
    /// </summary>
    public interface IDefaultColumn : IColumn
    {
        /// <summary>
        /// Returns the string representation of the column for Default Constraint purposes.
        /// </summary>
        /// <returns></returns>
        string GetDefaultString();
    }

    /// <summary>
    /// Foreign key column interface.
    /// </summary>
    public interface IForeignKeyColumn : IColumn
    {
    }

    /// <summary>
    /// Reference column interface.
    /// </summary>
    public interface IReferenceColumn : IColumn
    {
        /// <summary>
        /// Returns the string representation of the column for Foreign Key reference table
        /// purposes.
        /// </summary>
        /// <returns></returns>
        string GetForeignKeyReferenceString();
    }

    /// <summary>
    /// Primary key or unique index column.
    /// </summary>
    public interface IPrimaryKeyOrUniqueIndexColumn : IColumn
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
    /// Represents a Primary Key or Unique Index Column.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public interface IPrimaryKeyOrUniqueIndexColumn<out TParent>
        : IPrimaryKeyOrUniqueIndexColumn
        where TParent : IPrimaryKeyOrUniqueIndexColumn<TParent>
    {
    }

    /// <summary>
    /// Represents the Column concept.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public interface IColumn<out TParent>
        : IForeignKeyColumn
            , IReferenceColumn
            , IDefaultColumn
            , IPrimaryKeyOrUniqueIndexColumn<TParent>
            , IHasDataAttributes<IColumnAttribute, TParent>
            , IAlterIfExists<TParent>
        where TParent : IColumn<TParent>
    {
        /// <summary>
        /// Gets the FormattedType.
        /// </summary>
        /// <see cref="IDataTypeRegistry"/>
        string FormattedType { get; }

        /// <summary>
        /// Gets whether CanBeNull.
        /// </summary>
        bool? CanBeNull { get; }

        /// <summary>
        /// Gets whether Has Identity.
        /// </summary>
        /// <see cref="IIdentityColumnAttribute"/>
        /// <see cref="ISeededIdentityColumnAttribute"/>
        bool HasIdentity { get; }

        /// <summary>
        /// Gets the IdentitySeed if specified.
        /// </summary>
        int? IdentitySeed { get; }

        /// <summary>
        /// Gets the IdentityIncrement if specified.
        /// </summary>
        int? IdentityIncrement { get; }
    }

    /// <summary>
    /// Represents the Column concept.
    /// </summary>
    /// <typeparam name="TDbType"></typeparam>
    /// <typeparam name="TParent"></typeparam>
    public interface IColumn<TDbType, out TParent>
        : IColumn<TParent>
        where TParent : IColumn<TDbType, TParent>
    {
        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        TDbType Type { get; set; }
    }

    /// <summary>
    /// Represents the Column concept.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public abstract class ColumnBase<TParent>
        : DataBase<IColumnAttribute, TParent>
            , IColumn<TParent>
        where TParent : ColumnBase<TParent>
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
            get
            {
                bool result;
                return Attributes.TryFindColumnAttribute(out result, (NullableColumnAttribute x) => x.Value)
                    ? result
                    : (bool?) null;
            }
        }

        /// <summary>
        /// Gets whether Has Identity.
        /// </summary>
        public bool HasIdentity
        {
            get
            {
                return Attributes.TryColumnAttributeExists((IColumnAttribute c) =>
                    c is IIdentityColumnAttribute || c is ISeededIdentityColumnAttribute);
            }
        }

        /// <summary>
        /// Gets the IdentitySeed if specified.
        /// </summary>
        public int? IdentitySeed
        {
            get
            {
                int? result;
                return Attributes.TryFindColumnAttribute(out result, (SeededIdentityColumnAttribute x) => x.Value)
                    ? result
                    : null;
            }
        }

        /// <summary>
        /// Gets the IdentityIncrement if specified.
        /// </summary>
        public int? IdentityIncrement
        {
            get
            {
                int? result;
                return Attributes.TryFindColumnAttribute(out result, (SeededIdentityColumnAttribute x) => x.Increment)
                    ? result
                    : null;
            }
        }

        public TParent IfExists()
        {
            if (!Attributes.TryColumnAttributeExists((IIfExistsColumnAttribute c) => true))
                Attributes.Add(IfExistsColumnAttribute.DefaultInstance);
            return GetThisParent();
        }

        /// <summary>
        /// Gets whether Has If Exists clause.
        /// </summary>
        public bool HasIfExists
        {
            get { return Attributes.TryColumnAttributeExists((IIfExistsColumnAttribute c) => true); }
        }

        /// <summary>
        /// Returns the formatted Identity Seed string, with or without <see cref="IdentitySeed"/>
        /// and <see cref="IdentityIncrement"/>. Returns <see cref="string.Empty"/> when there is
        /// either no seed or no increment, both of which are required for this to appear.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetIdentitySeedString()
        {
            var seed = IdentitySeed;
            var increment = IdentityIncrement;

            return seed == null || increment == null
                ? string.Empty
                : string.Format(@"({0}, {1})", seed, increment);
        }

        /// <summary>
        /// Returns the formatted Identity string.
        /// </summary>
        /// <returns></returns>
        /// <see cref="GetIdentitySeedString"/>
        protected string GetIdentityString()
        {
            return HasIdentity
                ? string.Format(@" IDENTITY{0}", GetIdentitySeedString())
                : string.Empty;
        }

        /// <summary>
        /// Gets the Order.
        /// </summary>
        public SortOrder? Order
        {
            get
            {
                SortOrder result;
                return Attributes.TryFindColumnAttribute(out result, (SortOrderColumnAttribute x) => x.Value)
                    ? result
                    : (SortOrder?) null;
            }
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

        public abstract string GetDefaultString();

        public abstract string GetForeignKeyReferenceString();
    }

    /// <summary>
    /// Represents the Column concept.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    /// <typeparam name="TDbType"></typeparam>
    public abstract class ColumnBase<TDbType, TParent>
        : ColumnBase<TParent>
            , IColumn<TDbType, TParent>
        where TParent : ColumnBase<TDbType, TParent>
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
            var foundPrecision = Attributes.TryFindColumnAttribute(out precision,
                (PrecisionColumnAttribute x) => x.Value);

            var foundScale = Attributes.TryFindColumnAttribute(out scale,
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
