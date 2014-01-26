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
            
            //TODO: what's this about?
            // Work around nasty .NET framework bug
            try {
                new Uri("http://fail/first/time?only=%2bplus");
            } catch(Exception) {}

            Log.Info("Tally Code Camp - Starting");

            Error += delegate {
                var exception = Server.GetLastError();
                Log.Error(exception);

                Response.Clear();
                Server.ClearError();
                Response.Redirect("~/error");
            };

            //NOTE: some of this boilerplate stuff you'd get with a default File -> New Project 
            ContainerConfig.Configure();
            AreaRegistration.RegisterAllAreas(); //TODO: we're not using areas, so this line probably isn't needed
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            new RouteConfig(RouteTable.Routes).Configure(); //Note: this line deviates a bit from what you get in a brand new project

            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory());

            Log.Info("Tally Code Camp - Started");
        }

        protected void Session_Start(Object sender, EventArgs e) {
            Session["init"] = 0; //need to access session in order to get a consistent session id
        }
    }
}