using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EventSite.Domain;

namespace EventSite.Infrastructure.Filters {
    public class LoggedInAttribute : AuthorizeAttribute {
        public IApplicationState State { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext) {
            if(State.UserIsLoggedIn()) {
                var roles = splitString(Roles);
                return !roles.Any() || roles.Any(x => State.User.Roles.Contains(x));
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary {
                    {"controller", "Home"},
                    {"action", "Index"},
                    {"returnUrl", filterContext.HttpContext.Request.RawUrl}
                });
        }

        static string[] splitString(string original) {
            if(string.IsNullOrEmpty(original)) {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                let trimmed = piece.Trim()
                where !string.IsNullOrEmpty(trimmed)
                select trimmed.ToLower();

            return split.ToArray();
        }
    }
}