using System.Web.Mvc;

namespace EventSite.Infrastructure.Filters {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            //NOTE: best explanation of what this does that I could find: http://paulthecyclist.com/tag/handleerrorattribute/
            filters.Add(new HandleErrorAttribute());
        }
    }
}