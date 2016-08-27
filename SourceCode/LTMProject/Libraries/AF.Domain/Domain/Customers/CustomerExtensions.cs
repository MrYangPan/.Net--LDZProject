using System;
using System.Linq;

namespace AF.Domain.Domain.Customers
{
    public static class CustomerExtensions
    {
        #region Customer role

        /// <summary>
        /// Gets a value indicating whether customer is in a certain customer role
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="customerRoleSystemName">Customer role system name</param>
        /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsInCustomerRole(this Customers.Customer customer,
            string customerRoleSystemName, bool onlyActiveCustomerRoles = true)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (String.IsNullOrEmpty(customerRoleSystemName))
                throw new ArgumentNullException("customerRoleSystemName");

            var result = customer.CustomerRole.SystemName == customerRoleSystemName;
            return result;
        }




        /// <summary>
        /// Gets a value indicating whether customer is administrator
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsAdmin(this Customers.Customer customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, SystemCustomerRoleNames.Administrators, onlyActiveCustomerRoles);
        }


        #endregion

        #region Customer Power

        /// <summary>
        /// Determines whether [is in customer power] [the specified customer power system name].
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <param name="customerPowerSystemName">Name of the customer power system.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// customer
        /// or
        /// customerPowerSystemName
        /// </exception>
        public static bool IsInCustomerPower(this Customers.Customer customer,
            string customerPowerSystemName)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (String.IsNullOrEmpty(customerPowerSystemName))
                throw new ArgumentNullException("customerPowerSystemName");

            var result = customer.CustomerRole.PermissionRecords.FirstOrDefault(
                d => d.SystemName == customerPowerSystemName) != null;
            return result;
        }



        /// <summary>
        /// Haves the document collection power.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns></returns>
        public static bool HaveCollectionPower(this Customer customer)
        {
            return
                IsInCustomerPower(customer, SystemCustomerPowerNames.Collection);
        }

        /// <summary>
        /// Haves the entry power.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns></returns>
        public static bool HaveEntryPower(this Customer customer)
        {
            return
                IsInCustomerPower(customer, SystemCustomerPowerNames.Entry);
        }


        /// <summary>
        /// Haves the mark power.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns></returns>
        public static bool HaveMarkPower(this Customer customer)
        {
            return
                IsInCustomerPower(customer, SystemCustomerPowerNames.Mark);
        }

        /// <summary>
        /// Haves the review power.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns></returns>
        public static bool HaveReviewPower(this Customer customer)
        {
            return
                IsInCustomerPower(customer, SystemCustomerPowerNames.Review);
        }

        #endregion
    }
}
