using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using AF.Core;
using AF.Data.DbContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AF.Data.Tests
{
    /// <summary>
    /// Summary description for PersistenceTest
    /// </summary>
    public class PersistenceTest
    {
        private AFObjectContext _afObjectContext;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public AFObjectContext context
        {
            get { return _afObjectContext; }
            set { _afObjectContext = value; }
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void PersistenceInitialize()
        {
            context = new AFObjectContext(GetTestDbName());
            context.Database.Log = s => Debug.Print(s);
            Database.SetInitializer<AFObjectContext>(null);
        }

        protected string GetTestDbName()
        {
            string testDbName = "Data Source=172.16.2.53;Initial Catalog=LTM;Integrated Security=False;Persist Security Info=False;User ID=sa;Password=Jslx123456;MultipleActiveResultSets=True";
            return testDbName;
        }


        /// <summary>
        /// Persistance test helper
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="disposeContext">A value indicating whether to dispose context</param>
        protected T SaveAndLoadEntity<T>(T entity, bool disposeContext = true) where T : BaseEntity
        {

            context.Set<T>().Add(entity);
            context.SaveChanges();

            object id = entity.Id;

            if (disposeContext)
            {
                context.Dispose();
                context = new AFObjectContext(GetTestDbName());
            }

            var fromDb = context.Set<T>().Find(id);
            return fromDb;
        }
    }
}
