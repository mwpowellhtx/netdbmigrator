using System;

namespace Kingdom.Data
{
    /// <summary>
    /// Check constraint interface.
    /// </summary>
    public interface ICheckConstraint : IConstraint
    {
    }

    /// <summary>
    /// Check constraint interface.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public interface ICheckConstraint<out TParent> : ICheckConstraint
        where TParent : ICheckConstraint<TParent>
    {
        /// <summary>
        /// Signals Not For Replication attribute.
        /// </summary>
        TParent NotForReplication { get; }

        /// <summary>
        /// Sets the Logical Expression for the Check Constraint.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        TParent LogicalExpression(Func<string> expression);
    }

    /// <summary>
    /// Check Constraint base class.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public abstract class CheckConstraintBase<TParent>
        : ConstraintBase<TParent>
            , ICheckConstraint<TParent>
        where TParent : CheckConstraintBase<TParent>
    {
        public TParent NotForReplication
        {
            get { return InstallNotForReplication(); }
        }

        private TParent InstallNotForReplication()
        {
            Attributes.Add(NotForReplicationConstraintAttribute.Instance);
            return GetThisParent();
        }

        /// <summary>
        /// Gets whether Not For Replication constraint attribute is specified.
        /// </summary>
        /// <returns></returns>
        protected bool IsNotForReplicationSpecified
        {
            get
            {
                bool result;
                return Attributes.TryFindColumnAttribute(out result,
                    (NotForReplicationConstraintAttribute x) => result = true) && result;
            }
        }

        private Func<string> _expression;

        // TODO: not going to worry about fancier things like Linq-to-anything tree evalution or anything like that
        public TParent LogicalExpression(Func<string> expression)
        {
            _expression = expression;
            return GetThisParent();
        }

        /// <summary>
        /// Returns the 
        /// </summary>
        /// <returns></returns>
        protected string GetLogicalExpressionString()
        {
            return _expression().Trim();
        }

        /// <summary>
        /// Returns the string representing the Not For Replication attribute.
        /// </summary>
        /// <returns></returns>
        protected string GetNotForReplicationString()
        {
            return IsNotForReplicationSpecified ? @" NOT FOR REPLICATION" : string.Empty;
        }

        public override string GetAddableString()
        {
            var exprString = GetLogicalExpressionString();
            var notForReplicationString = GetNotForReplicationString();
            return string.Format(@"{0} {1} CHECK{2} ({3})",
                SubjectName, Name, notForReplicationString, exprString);
        }
    }
}
