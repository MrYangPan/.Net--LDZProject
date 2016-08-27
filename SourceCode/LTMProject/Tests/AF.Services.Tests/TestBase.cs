using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core.Infrastructure;
using AF.Data.DbContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AF.Services.Tests
{
    [TestClass]
   public class TestBase
    {
        [TestInitialize]
        public void SetUp()
        {
            EngineContext.Initialize(false);
        }

    }
}
