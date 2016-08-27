
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AF.Web.Models.Customer
{
    public class CustomerListModel
    {
        public CustomerListModel()
        {
            PagingFilteringContext = new CustomerListPagingFilteringModel();
            AvailableRoles = new List<SelectListItem>();
        }

        public IList<Domain.Domain.Customers.Customer> Customers { get; set; }
        public CustomerListPagingFilteringModel PagingFilteringContext { get; set; }

        public string UserName { get; set; }


        /// <summary>
        /// Gets or sets the customer role identifier.
        /// </summary>
        /// <value>
        /// The customer role identifier.
        /// </value>
        public int CustomerRoleId { get; set; }
        /// <summary>
        /// 角色SelectListItem
        /// </summary>
        /// <value>
        /// The available roles.
        /// </value>
        public IList<SelectListItem> AvailableRoles { get; set; }
        /// <summary>
        /// 状态选择id
        /// </summary>
        /// <value>
        /// The sid.
        /// </value>
        public int ActiveId { get; set; }
        /// <summary>
        /// 状态选择SelectListItem
        /// </summary>
        /// <value>
        /// The available status.
        /// </value>
        public IList<SelectListItem> AvailableActive { get; set; }

        public DateTime? BeginDate { get; set; }
  
        public DateTime? EndDate { get; set; }
        
    }
}
