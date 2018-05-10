using System.Web;
using System.Web.Optimization;

namespace GoPS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/js/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/js/jquery.unobtrusive*",
                        "~/js/jquery.validate.js",
                        "~/js/jquery.validate.date.js",
                        //"~/js/globalize*",
                        "~/js/messages_es.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/js/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/js/bootstrap.min.js",
                      "~/js/bootstrap.js",
                      "~/js/chart.min.js",
                      "~/js/chart-data.js",
                      "~/js/easypiechart.js",
                      "~/js/easypiechart-data.js",
                      "~/js/respond.min.js",
                      "~/js/bootstrap-table.js",
                      "~/js/bootstrap-table-filter.js",
                      "~/js/bootstrap-datepicker.js",
                      "~/js/bootstrap-datepicker-es.js",
                      "~/js/html5shiv.min.js",
                      "~/js/lumino.glyphs.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/css/bootstrap.min.css",
                      "~/css/bootstrap.css",
                      "~/css/bootstrap-table.css",
                      "~/css/bootstrap-theme.css",
                      "~/css/bootstrap-theme.css.map",
                      "~/css/bootstrap.css.map",
                      "~/css/datepicker.css",
                      "~/css/datepicker3.css",
                      "~/css/styles.css"));
        }
    }
}
