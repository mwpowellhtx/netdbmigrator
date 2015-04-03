using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kingdom.Data.Attributes;
using Kingdom.Data.Migrations;

namespace Kingdom.Data
{
    public class VersionInfo
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key,
         Column(TypeName = @"BIGINT"),
         Required]
        public virtual long Id { get; set; }

        /// <summary>
        /// Gets or sets the VersionId.
        /// </summary>
        [Column(TypeName = @"BIGINT"),
         Required]
        public virtual long VersionId { get; set; }

        /// <summary>
        /// Gets or sets the AppliedOn.
        /// </summary>
        [Column(TypeName = @"DATETIME"),
         Required]
        public virtual DateTime AppliedOn { get; set; }

        /// <summary>
        /// AttributeKind backing field.
        /// </summary>
        private AttributeKind? _attributeKind;

        /// <summary>
        /// Gets or sets the AttributeKind.
        /// </summary>
        [NotMapped]
        public AttributeKind? AttributeKind
        {
            get { return _attributeKind; }
            set { _attributeKind = value; }
        }

        //TODO: might not be a bad idea to have a first-class enum mapper handle this.
        [Column(@"AttributeKind", TypeName = @"NVARCHAR"),
         StringLength(9)]
        public string AttributeKindText
        {
            get
            {
                return AttributeKind.HasValue
                    ? AttributeKind.Value.ToString()
                    : null;
            }
            set
            {
                AttributeKind = string.IsNullOrEmpty(value)
                    ? null
                    : (AttributeKind?) Enum.Parse(typeof (AttributeKind), value);
            }
        }

        /// <summary>
        /// Gets or sets the Text.
        /// </summary>
        [Column(TypeName = @"NVARCHAR"),
         StringLength(127),
         Required]
        public virtual string Text { get; set; }

        /// <summary>
        /// Gets or sets the AttributeTypeFullName.
        /// </summary>
        [Column(TypeName = @"NVARCHAR"),
         StringLength(255),
         Required]
        public virtual string AttributeTypeFullName
        {
            get
            {
                return ReferenceEquals(null, AttributeType)
                    ? string.Empty
                    : AttributeType.FullName;
            }
            set
            {
                AttributeType = string.IsNullOrEmpty(value)
                    ? null
                    : Type.GetType(value);
            }
        }

        /// <summary>
        /// AttributeType backing field.
        /// </summary>
        private Type _attributeType;

        /// <summary>
        /// Gets or sets the AttributeType.
        /// </summary>
        [NotMapped]
        public virtual Type AttributeType
        {
            get { return _attributeType; }
            set
            {
                if (!ReferenceEquals(null, value))
                {
                    if (!(value == typeof(TimeStampMigrationAttribute)
                          || value == typeof(VersionMigrationAttribute)))
                    {
                        throw new InvalidOperationException(@"Invalid attribute type");
                    }
                }
                _attributeType = value;
            }
        }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        [Column(TypeName = @"NVARCHAR(MAX)")]
        public virtual string Description { get; set; }

        /// <summary>
        /// Sets the attribute.
        /// </summary>
        /// <param name="migration"></param>
        /// <param name="attrib"></param>
        internal void Configure(IMigration migration, AbstractMigrationAttribute attrib)
        {
            if (ReferenceEquals(null, attrib)) return;
            Description = migration.Description;
            VersionId = attrib.Id;
            Text = attrib.Text;
            AttributeKind = attrib.Kind;
            AttributeType = attrib.GetType();
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public VersionInfo()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes this object.
        /// </summary>
        private void Initialize()
        {
            Id = 0;
            VersionId = 0;
            AttributeKind = null;
            AppliedOn = DateTime.UtcNow;
        }
    }
}
