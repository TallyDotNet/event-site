using System.Web.Mvc;

namespace CodeCamp.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }
    }
}