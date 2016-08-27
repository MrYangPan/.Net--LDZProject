using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AF.Core;
using AF.Domain.Domain.BookWork;
using AF.Domain.Domain.Customers;
using AF.Domain.Infrastructure;
using AF.Services.BookWork;
using AF.Services.Message;
using AF.Web.Models.Common;


namespace AF.Web.Controllers
{
    [AllowAnonymous]
    public class CommonController : BasePublicController
    {
        private readonly IWorkContext _workContext;
        private readonly IBookWorkTaskService _bookWorkTaskService;

        public CommonController(IWorkContext workContext, IBookWorkTaskService bookWorkTaskService)
        {
            _workContext = workContext;
            _bookWorkTaskService = bookWorkTaskService;
        }


        //page not found
        public ActionResult PageNotFound()
        {
            this.Response.StatusCode = 404;
            this.Response.TrySkipIisCustomErrors = true;

            return View();
        }

        public ActionResult RemindInfo()
        {
            int? userid = null;
            if (!_workContext.CurrentCustomer.IsAdmin())
                userid = _workContext.CurrentCustomer.Id;
            var tasks = _bookWorkTaskService.GetAllTasks(userid, TaskStatus.Revert, null, 0, 10);
            var model = new RemindInfoModel {Tasks = tasks};
            return View(model);
        }


    }
}