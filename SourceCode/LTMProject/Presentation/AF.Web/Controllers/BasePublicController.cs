using System.Web.Mvc;
using System.Web.Routing;
using AF.Core.Infrastructure;
using AF.Domain.Infrastructure;
using AF.Services.Customers;
using AF.Web.Framework;

namespace AF.Web.Controllers
{
    [Authorize]
    public abstract partial class BasePublicController : BaseController
    {
        public BasePublicController()
        {
#if DEBUG
/*            var workcontext = EngineContext.Current.Resolve<IWorkContext>();
            var customerService = EngineContext.Current.Resolve<ICustomerService>();
            workcontext.CurrentCustomer = customerService.GetCustomerById(12);*/
#endif
        }



        protected virtual ActionResult InvokeHttp404()
        {
            // Call target Controller and pass the routeData.
            IController errorController = EngineContext.Current.Resolve<CommonController>();

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Common");
            routeData.Values.Add("action", "PageNotFound");

            errorController.Execute(new RequestContext(this.HttpContext, routeData));

            return new EmptyResult();
        }

    }
}
