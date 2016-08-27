using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AF.Domain.Domain.Customers;
using AF.Web.Framework;

namespace AF.Web.Models.Customer
{
    public class CustomerRolePermissionModel: BaseEntityModel
    {
        public int CustomerRoleId { get; set; }
        public IList<PermissionRecord> RolePermissionRecords { get; set; }
        public IList<PermissionRecord> PermissionRecords { get; set; }
        public IList<SelectListItem> AvailableRoles { get; set; }
        public IList<int> SelectPermission { get; set; }
    }
}
