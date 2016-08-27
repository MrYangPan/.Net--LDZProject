using System.Web.Mvc;

namespace AF.Admin.Controllers
{
    public class CommonController : BaseAdminController
    {




        //page not found
        public ActionResult PageNotFound()
        {
            this.Response.StatusCode = 404;
            this.Response.TrySkipIisCustomErrors = true;

            return View();
        }




    }
}