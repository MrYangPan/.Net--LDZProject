using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AF.Web.Validators.Customer;
using FluentValidation.Attributes;

namespace AF.Web.Models.Customer
{
    //[Validator(typeof(CustomerValidator))]
    public class CustomerModel
    {
        public Domain.Domain.Customers.Customer Customerbyid { get; set; }
        public int cid { get; set; }
        public IList<SelectListItem> AvailableRoles { get; set; }
    }
}
