using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using AF.Core;
using AF.Core.Caching;
using AF.Core.Data;
using AF.Core.Infrastructure;
using AF.Data;
using AF.Data.DbContext;
using AF.Domain.Domain.Customers;
using AF.Services.Events;
using AF.Services.Security;

namespace AF.Services.Customers
{
    /// <summary>
    /// Customer service
    /// </summary>
    public partial class CustomerService : ICustomerService
    {
        #region Constants


        /// <summary>
        /// The custome r_ al l_ key
        /// </summary>
        private const string CUSTOMER_KEY = "AF.customer.Id-{0}";


        #endregion

        #region Fields

        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerRole> _customerRoleRepository;
        private readonly IRepository<PermissionRecord> _permissionRecordRepository;

        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;

        private readonly ICacheManager _cacheManager;


        private readonly IEventPublisher _eventPublisher;


        #endregion

        #region Ctor

        public CustomerService(ICacheManager cacheManager,
            IRepository<Customer> customerRepository,
            IRepository<CustomerRole> customerRoleRepository,
            IRepository<PermissionRecord> permissionRecordRepository,
            IDataProvider dataProvider,
            IDbContext dbContext,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._customerRepository = customerRepository;
            this._customerRoleRepository = customerRoleRepository;
            this._permissionRecordRepository = permissionRecordRepository;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._eventPublisher = eventPublisher;


        }

        #endregion

        #region Methods


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
        public virtual IPagedList<Customer> GetAllCustormers(string userName, int roleId, bool? isActive,
            DateTime? beginDate,
            DateTime? endDate, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _customerRepository.Table.Where(d => !d.Deleted);

            if (!string.IsNullOrEmpty(userName))
                query = query.Where(c => c.Username.Contains(userName));

            if (roleId != 0)
                query = query.Where(c => c.CustomerRoleId == roleId);

            if (isActive.HasValue)
                query = query.Where(c => c.Active == isActive);

            if (beginDate.HasValue)
                query = query.Where(c => c.CreatedOn >= beginDate);

            if (endDate.HasValue)
                query = query.Where(c => c.CreatedOn <= endDate);

            query = query.OrderByDescending(c => c.CreatedOn);
            var customers = new PagedList<Customer>(query, pageIndex, pageSize);
            return customers;
        }


 

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>A customer</returns>
        public virtual Customer GetCustomerById(int customerId)
        {
            if (customerId == 0)
                return null;
            return _customerRepository.GetById(customerId);
        }



        /// <summary>
        /// Gets a customer by GUID
        /// </summary>
        /// <param name="customerGuid">Customer GUID</param>
        /// <returns>A customer</returns>
        public virtual Customer GetCustomerByGuid(Guid customerGuid)
        {
            if (customerGuid == Guid.Empty)
                return null;

            var query = from c in _customerRepository.Table
                where c.CustomerGuid == customerGuid && !c.Deleted
                orderby c.Id
                select c;
            var customer = query.FirstOrDefault();
            return customer;
        }



        /// <summary>
        /// Get customer by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Customer</returns>
        public virtual Customer GetCustomerByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.Username == username && !c.Deleted
                        select c;
            var customer = query.FirstOrDefault();
            return customer;
        }


        /// <summary>
        /// Insert a customer
        /// </summary>
        /// <param name="customer">Customer</param>
        public virtual void InsertCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            _customerRepository.Insert(customer);

            //event notification
            _eventPublisher.EntityInserted(customer);
        }

        /// <summary>
        /// Updates the customer
        /// </summary>
        /// <param name="customer">Customer</param>
        public virtual void UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            _customerRepository.Update(customer);

            //event notification
            _eventPublisher.EntityUpdated(customer);
        }


        #endregion

        #region CustomerRole


        /// <summary>
        /// Gets all customer roles.
        /// </summary>
        /// <returns></returns>
        public virtual IList<CustomerRole> GetAllCustomerRoles()
        {
            return _customerRoleRepository.Table.ToList();

        }


        /// <summary>
        /// Gets the customer role by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual CustomerRole GetCustomerRoleById(int id)
        {
            return _customerRoleRepository.GetById(id);
        }


        #endregion

        #region PermissionRecord


        /// <summary>
        /// Gets all permission records.
        /// </summary>
        /// <returns></returns>
        public virtual IList<PermissionRecord> GetAllPermissionRecords()
        {
            return _permissionRecordRepository.Table.ToList();
        }



        /// <summary>
        /// Updates the customer role.
        /// </summary>
        /// <param name="model">The model.</param>
        public virtual void UpdateCustomerRole(CustomerRole model)
        {
            _customerRoleRepository.Update(model);
        }

       

        #endregion
    }
}