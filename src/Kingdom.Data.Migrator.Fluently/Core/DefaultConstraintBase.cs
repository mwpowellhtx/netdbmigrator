using System;

namespace Kingdom.Data
{
    /// <summary>
    /// Default constraint interface.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public interface IDefaultConstraint<out TParent>
        : IConstraint
            , IHasDataAttributes<IConstraintAttribute, TParent>
        where TParent : IDefaultConstraint<TParent>
    {
        /// <summary>
        /// Gets the Column for Default Constraint purposes.
        /// </summary>
        IDefaultColumn Column { get; }

        /// <summary>
        /// Gets whether WithValues should be specified.
        /// </summary>
        bool WithValues { get; }

        /// <summary>
        /// Sets the <see cref="Column"/> for Default Constraint purposes.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        TParent For(IDefaultColumn column);

        // TODO: TBD: not sure I will need any other variations, like passing in generic T values, etc
        /// <summary>
        /// Sets the Constant Expression for the Default Constraint.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        TParent ConstantExpression(Func<string> expr);
    }

    /// <summary>
    /// Default Constraint base class.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public abstract class DefaultConstraintBase<TParent>
        : ConstraintBase<TParent>
            , IDefaultConstraint<TParent>
        where TParent : DefaultConstraintBase<TParent>
    {
        /// <summary>
        /// Gets or sets the Column.
        /// </summary>
        public IDefaultColumn Column { get; set; }

        public bool WithValues
        {
            get
            {
                bool result;
                return Attributes.TryFindColumnAttribute(out result,
                    (WithValuesConstraintAttribute x) => result = true) && result;
            }
        }

        public TParent For(IDefaultColumn column)
        {
            Column = column;
            return (TParent) this;
        }

        /// <summary>
        /// Constant Expression backing field.
        /// The default default value will be assumes to be Null when unspecified.
        /// </summary>
        private Func<string> _constantExpr = () => "NULL";

        public TParent ConstantExpression(Func<string> expr)
        {
            _constantExpr = expr;
            return (TParent) this;
        }

        private object _constantValue;

        /// <summary>
        /// Constant Expression backing field based on an injected parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constantValue"></param>
        /// <param name="formatter"></param>
        /// <returns></returns>
        public TParent ConstantExpression<T>(T constantValue, Func<T, string> formatter)
        {
            _constantValue = constantValue;
            _constantExpr = () => formatter((T) Convert.ChangeType(_constantValue, typeof (T)));
            return (TParent) this;
        }

        /// <summary>
        /// Returns the 
        /// </summary>
        /// <returns></returns>
        protected string GetConstantExpressionString()
        {
            return _constantExpr();
        }

        /// <summary>
        /// Returns the string representing the Witn Values attribute.
        /// </summary>
        /// <returns></returns>
        protected string GetWithValuesString()
        {
            return WithValues ? @" WITH VALUES" : string.Empty;
        }

        public override string GetAddableString()
        {
            var exprString = GetConstantExpressionString();

            var columnString = Column.GetDefaultString();

            var withValuesString = GetWithValuesString();

            return string.Format(@"{0} {1} DEFAULT {2} FOR {3}{4}",
                SubjectName, Name, exprString, columnString, withValuesString);
        }
    }
}
