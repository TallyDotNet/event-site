using System;
using System.Web.Optimization;

namespace EventSite.Infrastructure.Views {
    public class BundleConfig {
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            bundles.Add(
                new ScriptBundle("~/Scripts/combined")
                    .Include("~/Scripts/jquery-{version}.js")
                    .Include("~/Scripts/bootstrap.js")
                    .Include("~/Scripts/behavior.js")
                );

            bundles.Add(
                new StyleBundle("~/Content/styles/combined")
                    .Include("~/Content/styles/ie10mobile.css")
                    .Include("~/Content/bootstrap/bootstrap.min.css")
                    .Include("~/Content/styles/site.min.css")
                    .Include("~/Content/styles/font-awesome.css")
                    .Include("~/Content/styles/site.less")
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