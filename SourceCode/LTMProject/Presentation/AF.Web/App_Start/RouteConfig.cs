using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AF.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("HomePage",
                       "",
                       new { controller = "Home", action = "Index" },
                       new[] { "AF.Web.Controllers" });

            routes.MapRoute("Login",
                    "login",
                    new { controller = "Customer", action = "Login" },
                    new[] { "AF.Web.Controllers" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces:new []{ "AF.Web.Controllers" }
            );
        }
    }
}
