using System.Web;
using System.Web.Optimization;

namespace Signar
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery*",
                        "~/Scripts/semantic*",
                        "~/Scripts/chosen*"));

            bundles.Add(new ScriptBundle("~/bundles/navbar-menu").Include(
                        "~/Scripts/navbar-menu.js"));

            bundles.Add(new ScriptBundle("~/bundles/avatar").Include(
                      "~/Scripts/site.avatar.js"));

            bundles.Add(new ScriptBundle("~/bundles/jcrop").Include(
                      "~/Scripts/jquery.Jcrop.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryform").Include(
                      "~/Scripts/jquery.form.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap*",
                      "~/Scripts/respond.js",
                      "~/Scripts/dropdown.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap*",
                      "~/Content/site.css",
                      "~/Content/dropdown.css",
                      "~/Content/site.avatar.css",
                      "~/Content/chosen*"));

            bundles.Add(new StyleBundle("~/Content/navbar-menu").Include(
                      "~/Content/navbar-menu.css"));

            bundles.Add(new StyleBundle("~/Content/Login").Include(
                      "~/Content/Login.css"));

            bundles.Add(new StyleBundle("~/Content/asignar-styles").Include(
                      "~/Content/asignar-styles.css"));

            bundles.Add(new StyleBundle("~/Content/font-awesome").Include(
                      "~/Content/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/Popup/jquery-ui").Include(
                      "~/Content/Popup/jquery*",
                      "~/Content/Popup/theme.css",
                      "~/Content/semantic*",
                      "~/Content/jquery*"));

            bundles.Add(new StyleBundle("~/Content/popup").Include(
                "~/Content/popup.css"
                ));
        }
    }
}
