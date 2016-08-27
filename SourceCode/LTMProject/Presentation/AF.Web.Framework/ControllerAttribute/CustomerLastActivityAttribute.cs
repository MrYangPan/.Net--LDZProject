using System;
using System.Web.Mvc;
using AF.Core.Infrastructure;
using AF.Domain.Infrastructure;
using AF.Services.Customers;

namespace AF.Web.Framework.ControllerAttribute
{
    public class CustomerLastActivityAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext == null || filterContext.HttpContext == null || filterContext.HttpContext.Request == null)
                return;

            //don't apply filter to child methods
            if (filterContext.IsChildAction)
                return;

            //only GET requests
            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;

            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var customer = workContext.CurrentCustomer;
            if (customer == null)
                return;

            //update last activity date
            if (customer.LastActivityDate.AddMinutes(1.0) < DateTime.Now)
            {
                var customerService = EngineContext.Current.Resolve<ICustomerService>();
                customer.LastActivityDate = DateTime.Now;
                customerService.UpdateCustomer(customer);
            }
        }
    }
}
