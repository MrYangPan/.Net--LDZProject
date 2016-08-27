using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AF.Admin.Models.Customer;
using AF.Domain.Domain.Customers;
using AutoMapper;

namespace AF.Admin.Extensions
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return Mapper.Map(source, destination);
        }


        #region Customer

        public static CustomerModel ToModel(this Customer entity)
        {
            return entity.MapTo<Customer, CustomerModel>();
        }

        public static Customer ToEntity(this CustomerModel model)
        {
            return model.MapTo<CustomerModel, Customer>();
        }

        public static Customer ToEntity(this CustomerModel model, Customer destination)
        {
            return model.MapTo(destination);
        }

        #endregion




    }
}