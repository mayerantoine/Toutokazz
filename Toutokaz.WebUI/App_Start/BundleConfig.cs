using System.Web;
using System.Web.Optimization;

namespace Toutokaz.WebUI
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/admin.js",
                        "~/Scripts/bootstrap.min.js ",
                        "~/Scripts/jquery.fileupload-image.js",
                        "~/Scripts/jquery.fileupload-process",
                        "~/Scripts/jquery.fileupload-validate",
                        "~/Scripts/jquery.fileupload.js",
                        "~/Scripts/jquery.form.js",
                        "~/Scripts/jquery.iframe-transport.js",
                        "~/Scripts/jquery.slides.min.js",
                        "~/Scripts/respond.min.js"                  
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.ui.widget.js"              
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"                       
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css/AllMyCss.css").Include(
                            "~/Content/css/bootstrap.css",
                            "~/Content/css/select.css",
                            "~/Content/css/font-awesome.css",
                            "~/Content/css/czsale-responsive.css",
                            "~/Content/css/admin.css",
                            "~/Content/css/czsale-carousel.css"
                            ));

            bundles.Add(new StyleBundle("~/Content/themes/base").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}