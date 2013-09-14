using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace CodeCamp.Infrastructure.Routing {
    public class RouteBuilder {
        readonly RouteCollection routes;
        readonly string route;

        public RouteBuilder(RouteCollection routes, string route) {
            this.routes = routes;
            this.route = route;
        }

        public Route HandledBy<TController>(Expression<Func<TController, object>> action) {
            return AddRoute<TController>(action);
        }

        public Route HandledBy<TController>(Expression<Action<TController>> action) {
            return AddRoute<TController>(action);
        }

        Route AddRoute<TController>(LambdaExpression expression) {
            var controllerName = typeof(TController).Name.Replace("Controller", string.Empty);
            var actionName = ((MethodCallExpression) expression.Body).Method.Name;

            return routes.MapRoute(
                route,
                route,
                new {controller = controllerName, action = actionName}
                );
        }
    }
}