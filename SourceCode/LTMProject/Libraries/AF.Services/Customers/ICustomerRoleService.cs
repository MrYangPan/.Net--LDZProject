﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.Customer;

namespace AF.Services.Customers
{
    public interface ICustomerRoleService
    {
        IList<CustomerRole> GetAllCustomerRoles();

    }
}