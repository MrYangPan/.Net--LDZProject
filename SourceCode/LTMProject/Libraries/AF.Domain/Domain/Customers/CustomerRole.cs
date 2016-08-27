using System.Collections.Generic;
using AF.Core;

namespace AF.Domain.Domain.Customers
{
    /// <summary>
    /// Represents a customer role
    /// </summary>
    public class CustomerRole : BaseEntity, IKvEntity
    {
        /// <summary>
        /// Gets or sets the customer role name
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether the customer role is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer role is system
        /// </summary>
        public bool IsSystemRole { get; set; }

        /// <summary>
        /// Gets or sets the customer role system name
        /// </summary>
        public string SystemName { get; set; }

        public string Key => Name;
        public string Value => Id.ToString();
        private ICollection<PermissionRecord> _permissionRecord;
        public virtual ICollection<PermissionRecord> PermissionRecords
        {
            get { return _permissionRecord ?? (_permissionRecord = new List<PermissionRecord>()); }
            protected set { _permissionRecord = value; }
        }
    }

}