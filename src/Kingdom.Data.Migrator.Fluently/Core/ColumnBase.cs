using System;
using System.Collections.Generic;
using System.Linq;

namespace Kingdom.Data
{
    /// <summary>
    /// Represents the Subject of a Sql statement.
    /// </summary>
    public interface ISubject
    {
        /// <summary>
        /// Gets the SubjectName.
        /// </summary>
        string SubjectName { get; }
    }

    /// <summary>
    /// Represents the Column concept.
    /// </summary>
    public interface IColumn : ISubject, ITableAddable, ITableDroppable
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

        /// <summary>
        /// Adds the <paramref name="attributes"/> to the Column.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        IColumn Add(params IColumnAttribute[] attributes);
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

        /// <summary>
        /// Gets or sets the Attributes.
        /// </summary>
        IList<IColumnAttribute> Attributes { get; set; }
    }

    /// <summary>
    /// Represents the Column concept.
    /// </summary>
    public abstract class ColumnBase : IColumn
    {
        /// <summary>
        /// Gets the SubjectName: &quot;COLUMN&quot;
        /// </summary>
        public string SubjectName
        {
            get { return "COLUMN"; }
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
            return TryGetColumnAttribute(out result,
                (NullableColumnAttribute x) => x.CanBeNull)
                ? result
                : (bool?) null;
        }

        /// <summary>
        /// Gets the FormattedNullable string.
        /// </summary>
        protected string FormattedNullable
        {
            get
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
        }

        private IList<IColumnAttribute> _attributes;

        /// <summary>
        /// Gets or sets the Attributes.
        /// </summary>
        public IList<IColumnAttribute> Attributes
        {
            get { return _attributes; }
            set { _attributes = value ?? new List<IColumnAttribute>(); }
        }

        /// <summary>
        /// Protecte constructor.
        /// </summary>
        protected ColumnBase()
        {
            _attributes = new List<IColumnAttribute>();
        }

        /// <summary>
        /// Adds the <paramref name="attributes"/> to the Column.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public IColumn Add(params IColumnAttribute[] attributes)
        {
            foreach (var x in attributes) Attributes.Add(x);
            return this;
        }

        /// <summary>
        /// Tries to return the value of the attributed column through <paramref name="result"/>.
        /// If the attribute cannot be found, returns false.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result"></param>
        /// <param name="getter"></param>
        /// <returns></returns>
        protected bool TryGetColumnAttribute<TAttribute, TResult>(out TResult result,
            Func<TAttribute, TResult> getter)
            where TAttribute : IColumnAttribute
        {
            var attribute = _attributes.OfType<TAttribute>().SingleOrDefault();
            var found = attribute != null;
            result = found ? getter(attribute) : default(TResult);
            return found;
        }

        /// <summary>
        /// Returns the <see cref="ITableAddable"/> string.
        /// </summary>
        /// <returns></returns>
        public abstract string GetAddableString();

        /// <summary>
        /// Returns the <see cref="ITableDroppable"/> string.
        /// </summary>
        /// <returns></returns>
        public abstract string GetDroppableString();
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
            var foundPrecision = TryGetColumnAttribute(out precision,
                (PrecisionColumnAttribute x) => x.Precision);

            var foundScale = TryGetColumnAttribute(out scale,
                (ScaleColumnAttribute x) => x.Scale);

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
