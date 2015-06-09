using System;

namespace Kingdom.Data.Runners
{
    public interface IMigrationRunner<out TRunner, in TValue> : IDisposable
        where TValue : IComparable<TValue>
        where TRunner : IMigrationRunner<TRunner, TValue>
    {
        /// <summary>
        /// Migrates Up all versions.
        /// </summary>
        void Up();

        /// <summary>
        /// Migrates Down all versions.
        /// </summary>
        void Down();

        /// <summary>
        /// Migrates Up to the <see cref="maxValue"/>.
        /// </summary>
        /// <param name="maxValue"></param>
        void Up(TValue maxValue);

        /// <summary>
        /// Migrates Down to the <see cref="minValue"/>.
        /// </summary>
        /// <param name="minValue"></param>
        void Down(TValue minValue);

        /// <summary>
        /// Runs the runner given a delegated action.
        /// </summary>
        /// <param name="runner"></param>
        void Run(Action<TRunner> runner);
    }
}
