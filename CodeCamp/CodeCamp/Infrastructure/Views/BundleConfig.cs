using System;
using System.Web.Optimization;

namespace CodeCamp.Infrastructure.Views {
    public class BundleConfig {
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            bundles.Add(
                new ScriptBundle("~/Scripts/vendor.js")
                    .Include("~/Scripts/jquery-{version}.js")
                    .Include("~/Scripts/bootstrap.js")
                );

            bundles.Add(
                new StyleBundle("~/Content/css")
                    .Include("~/Content/styles/ie10mobile.css")
                    .Include("~/Content/bootstrap/bootstrap.min.css")
                    .Include("~/Content/styles/site.min.css")
                );
        }

        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList) {
            if(ignoreList == null) {
                throw new ArgumentNullException("ignoreList");
            }

            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            //ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }
    }
}