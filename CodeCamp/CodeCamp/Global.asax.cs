using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CodeCamp.App_Start;

namespace CodeCamp {
    public class MvcApplication : HttpApplication {
        protected void Application_Start() {
            ContainerConfig.Configure();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}