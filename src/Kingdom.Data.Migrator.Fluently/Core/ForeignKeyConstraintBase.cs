using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Kingdom.Data
{
    /// <summary>
    /// Foreign key constraint interface.
    /// </summary>
    public interface IForeignKeyConstraint : IConstraint
    {
    }

    /// <summary>
    /// Represents the References part of the Foreign Key fluent sentence.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public interface IForeignKeyReference<out TParent>
        : IHasColumns<IReferenceColumn, TParent>
    {
        /// <summary>
        /// Sets the Reference Table <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks>Returning the Foreign Key Reference instance is on purpose, which
        /// allows closing the fluent chain with the columns, and then returning to the
        /// parent constraint to complete the fluent sentence.</remarks>
        IForeignKeyReference<TParent> Table(INamePath name);
    }

    /// <summary>
    /// Foreign key reference class.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public class ForeignKeyReference<TParent>
        : IForeignKeyReference<TParent>
    {
        private INamePath _tableName;

        /// <summary>
        /// Gets the <see cref="_tableName"/> for internal use.
        /// </summary>
        internal INamePath InternalTableName
        {
            get { return _tableName; }
        }

        public IForeignKeyReference<TParent> Table(INamePath name)
        {
            _tableName = name;
            return this;
        }

        private readonly IFluentCollection<IReferenceColumn, TParent> _columns;

        /// <summary>
        /// Gets the <see cref="IFluentCollection{IReferenceColumn,TParent}.Items"/> for internal use.
        /// </summary>
        internal IReadOnlyCollection<IReferenceColumn> InternalColumns
        {
            get { return new ReadOnlyCollection<IReferenceColumn>(_columns.Items); }
        }

        public IFluentCollection<IReferenceColumn, TParent> Columns
        {
            get { return _columns; }
        }

        internal ForeignKeyReference(TParent parent)
        {
            _columns = new FluentCollection<IReferenceColumn, TParent>(parent);
        }
    }

    /// <summary>
    /// Represents a foreign key constraint.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public interface IForeignKeyConstraint<out TParent>
        : IForeignKeyConstraint
            , IHasColumns<IForeignKeyColumn, TParent>
        where TParent : IForeignKeyConstraint<TParent>
    {
        /// <summary>
        /// Gets the Foreign Key References part.
        /// </summary>
        IForeignKeyReference<TParent> References { get; }

        /// <summary>
        /// Sets the action triggered by <see cref="ForeignKeyTrigger.Delete"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        TParent OnDelete(ForeignKeyAction action);

        /// <summary>
        /// Sets the action triggered by <see cref="ForeignKeyTrigger.Update"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        TParent OnUpdate(ForeignKeyAction action);

        /// <summary>
        /// Signals that the Foreign Key constraint is Not For Replication.
        /// </summary>
        TParent NotForReplication { get; }
    }

    /// <summary>
    /// Foreign key constraint.
    /// </summary>
    /// <typeparam name="TParent"></typeparam>
    public abstract class ForeignKeyConstraintBase<TParent>
        : ConstraintBase<TParent>
            , IForeignKeyConstraint<TParent>
        where TParent : ForeignKeyConstraintBase<TParent>
    {
        private readonly IFluentCollection<IForeignKeyColumn, TParent> _columns;

        public IFluentCollection<IForeignKeyColumn, TParent> Columns
        {
            get { return _columns; }
        }

        private readonly ForeignKeyReference<TParent> _reference;

        public IForeignKeyReference<TParent> References
        {
            get { return _reference; }
        }

        public TParent OnDelete(ForeignKeyAction action)
        {
            return OnTrigger(ForeignKeyTrigger.Delete, action);
        }

        public TParent OnUpdate(ForeignKeyAction action)
        {
            return OnTrigger(ForeignKeyTrigger.Update, action);
        }

        private TParent OnTrigger(ForeignKeyTrigger trigger, ForeignKeyAction action)
        {
            return Set(trigger, () => new ForeignKeyActionConstraintAttribute(action), x => x.Value == trigger);
        }

        public TParent NotForReplication
        {
            get { return Set(() => new NotForReplicationConstraintAttribute()); }
        }

        /// <summary>
        /// Protected default constructor.
        /// </summary>
        protected ForeignKeyConstraintBase()
        {
            {
                var parent = GetThisParent();
                _reference = new ForeignKeyReference<TParent>(parent);
                _columns = new FluentCollection<IForeignKeyColumn, TParent>(parent);
            }
        }

        /// <summary>
        /// Returns the <see cref="ForeignKeyAction"/> corresponding to the desired
        /// <paramref name="trigger"/>, if it exists. Returns Null when one does not exist.
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        protected ForeignKeyAction? GetTriggeredActionValue(ForeignKeyTrigger trigger)
        {
            ForeignKeyAction? result;
            return Attributes.TryFindColumnAttribute(out result,
                (ForeignKeyActionConstraintAttribute x) => x.Action,
                x => x.Value == trigger)
                ? result
                : null;
        }

        /// <summary>
        /// Returns the Column strings.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<string> GetColumnStrings()
        {
            return Columns.Items.Select(x => x.Name.ToString());
        }

        /// <summary>
        /// Returns the string representation for the <paramref name="trigger"/> action.
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        protected string GetTriggeredActionString(ForeignKeyTrigger trigger)
        {
            var action = GetTriggeredActionValue(trigger);

            Func<string, string> formatter = a => string.Format(@" on {0} {1}", trigger, a).ToUpper();

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (action)
            {
                case ForeignKeyAction.NoAction:
                    return formatter("no action");

                case ForeignKeyAction.Cascade:
                    return formatter("cascade");

                case ForeignKeyAction.SetNull:
                    return formatter("set null");

                case ForeignKeyAction.SetDefault:
                    return formatter("set default");

                case null:
                    // Remember the null case is valid meaning no triggered action was specified.
                    return string.Empty;
            }

            throw this.ThrowNotSupportedException(
                () => string.Format("Action ({0}) not supported for ON {1} trigger.",
                    action == null ? "null" : action.Value.ToString(),
                    trigger.ToString().ToUpper()));
        }

        /// <summary>
        /// Returns the string representation of the the Reference Table Name.
        /// </summary>
        /// <returns></returns>
        protected string GetReferenceTableNameString()
        {
            return _reference.InternalTableName.ToString();
        }

        /// <summary>
        /// Returns the string representation of the Reference Columns.
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<string> GetReferenceColumnStrings()
        {
            return _reference.InternalColumns.Select(x => x.Name.ToString());
        }

        /// <summary>
        /// Gets whether Not For Replication constraint attribute is specified.
        /// </summary>
        /// <returns></returns>
        protected bool IsNotForReplicationSpecified
        {
            get
            {
                return Attributes.TryColumnAttributeExists(
                    (IConstraintAttribute c) => c is INotForReplicationConstraintAttribute);
            }
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
            var columnString = CommaDelimited(GetColumnStrings());

            var referenceTableName = GetReferenceTableNameString();
            var referenceColumnStrings = CommaDelimited(GetReferenceColumnStrings());

            var onDeleteString = GetTriggeredActionString(ForeignKeyTrigger.Delete);
            var onUpdateString = GetTriggeredActionString(ForeignKeyTrigger.Update);

            var notForReplicationString = GetNotForReplicationString();

            return string.Format(@"{0} {1} FOREIGN KEY ({2}) REFERENCES {3} ({4}){5}{6}{7}",
                SubjectName, Name, columnString, referenceTableName, referenceColumnStrings,
                onDeleteString, onUpdateString, notForReplicationString);
        }
    }

    internal static class ForeignKeyReferenceExtensionMethods
    {

    }
}
