using System.Web;
using System.Web.Optimization;

namespace AF.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Content/Scripts/Jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Content/Scripts//Jquery/JqueryValidate/jquery.validate*"));

            bundles.Add(new StyleBundle("~/bundles/css/testSheet").Include(
                "~/Content/CSS/Examination/Examination-new.css"));
        }
    }
}
