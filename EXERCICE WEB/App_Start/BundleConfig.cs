using System.Web;
using System.Web.Optimization;

namespace PORTAIL_MIZA
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            #region template
            bundles.Add(new ScriptBundle("~/template/js").Include(
                       "~/Scripts/jquery.js",
                       "~/Scripts/bootstrap.min.js",
                       "~/Scripts/jquery.scrollTo.min.js",
                       "~/Scripts/jquery.nicescroll.js",
                       "~/Scripts/jquery.sparkline.js",
                    
                       "~/Scripts/owl.carousel.js",
                       "~/Scripts/fullcalendar.min.js",
                       "~/Scripts/fullcalendar.js",
                       "~/Scripts/calendar-custom.js",
                
                       "~/Scripts/Chart.js",
                       "~/Scripts/scripts.js",
                       "~/Scripts/sparkline-chart.js",
                       "~/Scripts/easy-pie-chart.js",
                  
                       "~/Scripts/xcharts.min.js",
                     
                       "~/Scripts/gdp-data.js",
                       "~/Scripts/morris.min.js",
                       "~/Scripts/sparklines.js",
                       "~/Scripts/charts.js",
                      "~/Scripts/jquery.slimscroll.min.js"
                       ));

            bundles.Add(new StyleBundle("~/template/css").Include(
                     "~/Content/bootstrap.min.css",

                     "~/Content/bootstrap - theme.css",
                      "~/Content/css/elegant-icons-style.css",
                      "~/Content/css/font-awesome.min.css",
                      "~/Content/css/owl.carousel.css",
                      "~/Content/css/jquery-jvectormap-1.2.2..css",
                      "~/Content/css/fullcalendar.css",
                       "~/Content/assets/fullcalendar/fullcalendar/bootstrap-fullcalendar.css",
                        "~/Content/assets/fullcalendar/fullcalendar/fullcalendar.css",
                         "~/Content/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css",
                       
                      "~/Content/css/widgets.css",
                      "~/Content/css/style.css",
                      "~/Content/css/style-responsive.css",
                      "~/Content/css/xcharts.min.css", "~/Content/site.css"
                     ));


                   




            #endregion
        }
    }
}
