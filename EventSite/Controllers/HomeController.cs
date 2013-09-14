using System.Web.Mvc;
using EventSite.Infrastructure.Controllers;

namespace EventSite.Controllers {
    public class HomeController : BaseController {
        public ActionResult Index() {
            return View();
        }
    }
}