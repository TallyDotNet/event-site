using System.Web.Mvc;
using System.Web.Routing;
using EventSite.Controllers;
using EventSite.Domain;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using EventSite.Infrastructure.Views;
using EventSite.ViewModels;
using NLog;

namespace EventSite.Infrastructure.Controllers {
    public abstract class LowLevelController : Controller {
        //static readonly ITempDataProvider CookieTempData = new CookieTempDataProvider();

        public Logger Log { get; set; }
        public ViewInfo ViewInfo { get; private set; }
        public IApplicationState State { get; private set; }
        public IApplicationBus Bus { get; private set; }

        protected User CurrentUser {
            get { return State.User; }
        }

        protected LowLevelController() {
            ViewInfo = DependencyResolver.Current.GetService<ViewInfo>();
            State = DependencyResolver.Current.GetService<IApplicationState>();
            Bus = DependencyResolver.Current.GetService<IApplicationBus>();
        }

        //TODO: doesn't look like we're using this
        //protected override ITempDataProvider CreateTempDataProvider() {
        //    return CookieTempData;
        //}

        protected override void HandleUnknownAction(string actionName) {
            if(GetType() != typeof(ErrorController)) {
                var errorController = DependencyResolver.Current.GetService<ErrorController>();
                var errorRoute = new RouteData();
                errorRoute.Values.Add("controller", "Error");
                errorRoute.Values["action"] = "Error";
                errorRoute.Values["url"] = HttpContext.Request.Url.OriginalString;
                errorRoute.Values["errorCode"] = "404";
                errorRoute.Values["message"] = "Not Found";
                errorController.Execute(new RequestContext(HttpContext, errorRoute));
            }
        }

        protected internal ActionResult NotFound() {
            return CreateErrorActionResult(404, "Not Found");
        }

        protected internal ActionResult Forbidden() {
            return CreateErrorActionResult(403, "Forbidden");
        }

        protected internal ActionResult CreateErrorActionResult(int statusCode = 500, string message = "Server Error", string url = null) {
            Response.Clear();
            Response.StatusCode = statusCode;
            return View("Error", new ErrorOutput {
                ErrorCode = statusCode.ToString(),
                Message = message,
                Url = url
            });
        }

        protected HttpStatusCodeResult Success() {
            return new HttpStatusCodeResult(200);
        }
    }
}