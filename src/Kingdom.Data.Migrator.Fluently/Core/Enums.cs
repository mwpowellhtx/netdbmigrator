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
    /// Table index type enumeration.
    /// </summary>
    public enum TableIndexType
    {
        /// <summary>
        /// PRIMARY KEY
        /// </summary>
        PrimaryKey,

        /// <summary>
        /// UNIQUE
        /// </summary>
        UniqueIndex
    }

    /// <summary>
    /// Foreign key trigger enumeration.
    /// </summary>
    public enum ForeignKeyTrigger
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

    /// <summary>
    /// Foreign key action enumeration.
    /// </summary>
    public enum ForeignKeyAction
    {
        /// <summary>
        /// NO ACTION
        /// </summary>
        NoAction,

        /// <summary>
        /// CASCADE
        /// </summary>
        Cascade,

        /// <summary>
        /// SET NULL
        /// </summary>
        SetNull,

        /// <summary>
        /// SET DEFAULT
        /// </summary>
        SetDefault
    }

    /// <summary>
    /// Sort order enumeration.
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// ASC
        /// </summary>
        Ascending,

        /// <summary>
        /// DESC
        /// </summary>
        Descending
    }

    /// <summary>
    /// Clustered type enumeration.
    /// </summary>
    public enum ClusteredType
    {
        /// <summary>
        /// 
        /// </summary>
        Clustered,

        /// <summary>
        /// 
        /// </summary>
        NonClustered
    }
}
