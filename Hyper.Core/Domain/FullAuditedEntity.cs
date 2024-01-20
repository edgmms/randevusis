using System;

namespace Hyper.Core.Domain
{
    /// <summary>
    /// Defines the <see cref="FullAuditedEntity" />.
    /// </summary>
    public partial class FullAuditedEntity<T> : BaseEntity<T>, ISoftDeletedEntity
    {
        /// <summary>
        /// Gets or sets the CreatedById.
        /// </summary>
        public T CreatedById { get; set; }

        /// <summary>
        /// Gets or sets the CreatedOnUtc.
        /// </summary>
        public DateTime CreatedOnUtc { get; set; } 

        /// <summary>
        /// Gets or sets the UpdatedById.
        /// </summary>
        public T UpdatedById { get; set; }

        /// <summary>
        /// Gets or sets the UpdatedOnUtc.
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the DeletedById.
        /// </summary>
        public T DeletedById { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Deleted.
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the DeletedOnUtc.
        /// </summary>
        public DateTime? DeletedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Active.
        /// </summary>
        public bool Active { get; set; }
    }
}
