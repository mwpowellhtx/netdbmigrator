using System;

namespace Kingdom.Data.Attributes
{
    public abstract class AbstractMigrationAttribute : Attribute
    {
        /// <summary>
        /// Gets an Id from the attribute.
        /// </summary>
        internal abstract long Id { get; }

        /// <summary>
        /// Gets the Kind.
        /// </summary>
        internal abstract AttributeKind Kind { get; }

        /// <summary>
        /// Gets or sets the DecoratedType.
        /// </summary>
        internal Type DecoratedType { get; set; }

        /// <summary>
        /// Gets the Text from the attribute.
        /// </summary>
        internal abstract string Text { get; }

        /// <summary>
        /// Gets the Value.
        /// </summary>
        internal object Value { get; private set; }

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="value"></param>
        protected AbstractMigrationAttribute(object value)
        {
            Value = value;

            Initialize();
        }

        /// <summary>
        /// Initializes the attribute.
        /// </summary>
        private void Initialize()
        {
            Verify();
        }

        /// <summary>
        /// Verifies that the attribute is valid for use.
        /// </summary>
        internal abstract void Verify();
    }
}
