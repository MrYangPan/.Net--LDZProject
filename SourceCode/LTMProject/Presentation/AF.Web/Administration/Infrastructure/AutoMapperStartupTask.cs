using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Admin.Models.Customer;
using AF.Core.Infrastructure;
using AF.Domain.Domain.Customers;
using AutoMapper;

namespace AF.Admin.Infrastructure
{
    public class AutoMapperStartupTask : IStartupTask
    {
        public void Execute()
        {



            Mapper.CreateMap<Customer, CustomerModel>();
            Mapper.CreateMap<CustomerModel, Customer>();



        }

        public int Order
        {
            get { return 0; }
        }
    }
}