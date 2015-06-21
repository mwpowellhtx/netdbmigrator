using System;
using Kingdom.Data.Runners;

namespace Kingdom.Data.Planners
{
    /// <summary>
    /// Migration plan base class.
    /// </summary>
    /// <typeparam name="TRunner"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Obsolete]//TODO: this was an artifact of some internal work I was doing; but not sure it's worth it for the 10 or so lines of code this is saving me
    public abstract class MigrationPlanBase<TRunner, TValue> : IMigrationPlan
        where TValue : IComparable<TValue>
        where TRunner : IMigrationRunner<TRunner, TValue>
    {
        /// <summary>
        /// Gets the ConnectionString.
        /// </summary>
        protected string ConnectionString { get; private set; }

        /// <summary>
        /// RunnerFactory backing field.
        /// </summary>
        private readonly MigrationRunnerFactory<TRunner, TValue> _runnerFactory;

        /// <summary>
        /// Run backing field.
        /// </summary>
        private readonly Action<TRunner> _run;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="runnerFactory"></param>
        /// <param name="run"></param>
        protected MigrationPlanBase(
            string connectionString,
            MigrationRunnerFactory<TRunner, TValue> runnerFactory,
            Action<TRunner> run)
        {
            ConnectionString = connectionString;
            _runnerFactory = runnerFactory;
            _run = run;
        }

        /// <summary>
        /// Migrates the database.
        /// </summary>
        internal void Migrate()
        {
            //TODO: these are the core lines of code this is saving: like I said, really not worth it...
            using (var runner = _runnerFactory(ConnectionString))
            {
                _run(runner);
            }
        }

        #region Disposable Members

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public virtual void Dispose()
        {
            Migrate();
        }

        #endregion
    }
}
