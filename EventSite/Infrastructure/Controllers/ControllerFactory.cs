using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EventSite.Controllers;

namespace EventSite.Infrastructure.Controllers {
    public class ControllerFactory : DefaultControllerFactory {
        public override IController CreateController(RequestContext requestContext, string controllerName) {
            try {
                return base.CreateController(requestContext, controllerName);
            } catch(HttpException ex) {
                var errorRoute = requestContext.RouteData;
                errorRoute.Values["controller"] = "Error";
                errorRoute.Values["action"] = "Error";
                errorRoute.Values["url"] = requestContext.HttpContext.Request.Url.OriginalString;
                errorRoute.Values["errorCode"] = ex.GetHttpCode();
                errorRoute.Values["message"] = "Http Error";
                return DependencyResolver.Current.GetService<ErrorController>();
            }
        }
    }
}