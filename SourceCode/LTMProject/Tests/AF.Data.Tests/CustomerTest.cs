using System;
using System.Text;
using System.Collections.Generic;
using AF.Domain.Domain.Customers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace AF.Data.Tests
{
    /// <summary>
    /// Summary description for CustomerTest
    /// </summary>
    [TestClass]
    public class CustomerTest: PersistenceTest
    {


        [TestMethod]
        public void Can_save_and_load_customer()
        {
            var customer = GetTestCustomer();

            var fromDb = SaveAndLoadEntity(customer);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(fromDb.Username, "a@b.com");
            Assert.AreEqual(fromDb.Password, "password");
            Assert.AreEqual(fromDb.Active,true);
            Assert.AreEqual(fromDb.CreatedOn,new DateTime(2010, 01, 01));
        }



        protected Customer GetTestCustomer()
        {
            return new Customer
            {
                Username = "a@b.com",
                Password = "password",
                PasswordFormat = PasswordFormat.Clear,
                PasswordSalt = "",
                Email = "a@b.com",
                CustomerGuid = Guid.NewGuid(),

                Active = true,
                Deleted = false,
  
                LastIpAddress = "192.168.1.1",
                CreatedOn = new DateTime(2010, 01, 01),
                LastLoginDate = new DateTime(2010, 01, 02),
                LastActivityDate = new DateTime(2010, 01, 03)
            };
        }
    }
}
