using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace CodeCamp.Infrastructure.Routing {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Slugged",
                "{controller}/{slug}/{action}"
                );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );

            routes.MapRoute(
                "NotFound",
                "{*url}",
                new {controller = "Error", action = "Error", errorCode = 404, message = "Not Found"}
                );
        }
    }
}