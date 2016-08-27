using System.Collections.Generic;
using AF.Core;

namespace AF.Domain.Domain.Customers
{
    public partial class PermissionRecord : BaseEntity
    {
        public string Name { get; set; }
        public string SystemName { get; set; }

        private ICollection<CustomerRole> _customerRoles;
        /// <summary>
        /// Gets or sets the customer roles
        /// </summary>
        public virtual ICollection<CustomerRole> CustomerRoles
        {
            get { return _customerRoles ?? (_customerRoles = new List<CustomerRole>()); }
            protected set { _customerRoles = value; }
        }
    }
}
