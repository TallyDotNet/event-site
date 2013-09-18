using System;
using System.Web;

namespace EventSite.Infrastructure.Routing {
    public static class RouteHelper {
        public static string GetReturnUrl(string url) {
            if(string.IsNullOrEmpty(url)) {
                return null;
            }

            var uri = new Uri(url);
            return HttpUtility.ParseQueryString(uri.Query).Get("returnUrl");
        }
    }
}