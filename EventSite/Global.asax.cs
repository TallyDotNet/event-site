using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EventSite.Infrastructure.Controllers;
using EventSite.Infrastructure.Filters;
using EventSite.Infrastructure.IoC;
using EventSite.Infrastructure.Routing;
using EventSite.Infrastructure.Views;
using NLog;

namespace EventSite {
    public class MvcApplication : HttpApplication {
        static readonly Logger Log = LogManager.GetLogger(typeof(MvcApplication).FullName);

        protected void Application_Start() {
            Log.Info("Event Site - Starting");

            Error += delegate {
                var exception = Server.GetLastError();
                Log.Error(exception);

                Response.Clear();
                Server.ClearError();
                Response.Redirect("~/error");
            };

            ContainerConfig.Configure();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            new RouteConfig(RouteTable.Routes).Configure(); //Note: this line deviates a bit from what you get in a brand new project

            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory());

            Log.Info("Event Site - Started");
        }

        protected void Session_Start(Object sender, EventArgs e) {
            //we'll rely on session later for authentication/authorization, so we init it here to ensure
            //that we get an keep the same session Id all the way through.
            Session["init"] = 0;
        }
    }
}