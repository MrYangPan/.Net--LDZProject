using System;
using System.Collections.Generic;
using AF.Core;
using AF.Domain.Domain.Customers;

namespace AF.Services.Customers
{
    /// <summary>
    /// Customer service interface
    /// </summary>
    public partial interface ICustomerService
    {
        #region Customers





        /// <summary>
        /// Gets all custormers.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="isActive">The is active.</param>
        /// <param name="beginDate">The begin date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        IPagedList<Customer> GetAllCustormers(string userName, int roleId, bool? isActive, DateTime? beginDate,
            DateTime? endDate, int pageIndex = 0, int pageSize = int.MaxValue);


        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>A customer</returns>
        Customer GetCustomerById(int customerId);



        /// <summary>
        /// Gets a customer by GUID
        /// </summary>
        /// <param name="customerGuid">Customer GUID</param>
        /// <returns>A customer</returns>
        Customer GetCustomerByGuid(Guid customerGuid);





        /// <summary>
        /// Get customer by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Customer</returns>
        Customer GetCustomerByUsername(string username);


        /// <summary>
        /// Insert a customer
        /// </summary>
        /// <param name="customer">Customer</param>
        void InsertCustomer(Customer customer);

        /// <summary>
        /// Updates the customer
        /// </summary>
        /// <param name="customer">Customer</param>
        void UpdateCustomer(Customer customer);




        #endregion


        #region Customer Permission

        /// <summary>
        /// Gets all permission records.
        /// </summary>
        /// <returns></returns>
        IList<PermissionRecord> GetAllPermissionRecords();



        #endregion


        #region CustomerRole

        /// <summary>
        /// Gets all customer roles.
        /// </summary>
        /// <returns></returns>
        IList<CustomerRole> GetAllCustomerRoles();


        /// <summary>
        /// Updates the customer role.
        /// </summary>
        /// <param name="model">The model.</param>
        void UpdateCustomerRole(CustomerRole model);


        /// <summary>
        /// Gets the customer role by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        CustomerRole GetCustomerRoleById(int id);

        #endregion
    }
}