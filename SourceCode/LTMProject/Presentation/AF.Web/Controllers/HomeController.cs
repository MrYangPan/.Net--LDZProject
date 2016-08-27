using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AF.Core.Infrastructure;
using AF.Domain.Domain.Customers;
using AF.Domain.Infrastructure;
using AF.Services.Customers;

using AF.Web.Framework.ControllerAttribute;

namespace AF.Web.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    public class HomeController : BasePublicController
    {
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _workContext;

        #region Fields



        #endregion

        #region Ctor

        public HomeController(ICustomerService customerService, IWorkContext workContext)
        {
            _customerService = customerService;
            _workContext = workContext;
        }

        #endregion


        public ActionResult Index()
        {
            return View();
        }


    }
}