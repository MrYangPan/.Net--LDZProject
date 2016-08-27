using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AF.Web.Framework;
using AF.Web.Validators.Customer;
using FluentValidation.Attributes;

namespace AF.Web.Models.Customer
{
    [Validator(typeof(CustomerUpdateValidator))]
    public class CustomerUpdateModel : BaseEntityModel
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string RealName { get; set; }
        public string PassWord { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        public IList<SelectListItem> AvailableRoles { get; set; }
    }
}
