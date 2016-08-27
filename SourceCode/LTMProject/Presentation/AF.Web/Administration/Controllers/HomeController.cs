using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AF.Admin.Extensions;
using AF.Services.Customers;

namespace AF.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        #region Fields

        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public HomeController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        #endregion

        public ActionResult Index()
        {
            var customer = _customerService.GetCustomerById(10);

            var customermodel = customer.ToModel();
            var customerentity = customermodel.ToEntity();
            customermodel.ToEntity(customer);


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}