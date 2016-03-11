namespace Kingdom.Data
{
    /// <summary>
    /// Check type enumeration.
    /// </summary>
    public enum CheckType
    {
        /// <summary>
        /// CHECK
        /// </summary>
        Check,

        /// <summary>
        /// NOCHECK
        /// </summary>
        NoCheck
    }

    /// <summary>
    /// Alter table type enumeration.
    /// </summary>
    public enum AlterTableType
    {
        /// <summary>
        /// ADD
        /// </summary>
        Add,

        /// <summary>
        /// DROP
        /// </summary>
        Drop
    }

    /// <summary>
    /// Foreign key event enumeration.
    /// </summary>
    public enum ForeignKeyEvent
    {
        /// <summary>
        /// On Delete
        /// </summary>
        Delete,

        /// <summary>
        /// On Update
        /// </summary>
        Update
    }
}
