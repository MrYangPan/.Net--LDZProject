using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Web.Framework;

namespace AF.Admin.Models.Customer
{
    public class CustomerModel: BaseEntityModel
    {
        /// <summary>
        /// Gets or sets the customer Guid
        /// </summary>
        public Guid CustomerGuid { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the password
        /// </summary>
        public string Password { get; set; }
    }
}