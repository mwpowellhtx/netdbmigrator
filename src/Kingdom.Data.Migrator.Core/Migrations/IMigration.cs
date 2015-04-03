using System;

namespace Kingdom.Data.Migrations
{
    /// <summary>
    /// Migration interface.
    /// </summary>
    public interface IMigration : IDisposable
    {
        /// <summary>
        /// Gets the Description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Upgrades the database.
        /// </summary>
        void Up();

        /// <summary>
        /// Downgrades the database.
        /// </summary>
        void Down();
    }
}
