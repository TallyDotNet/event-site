using System.Web.Mvc;
using CodeCamp.Infrastructure.Controllers;

namespace CodeCamp.Controllers {
    public class HomeController : BaseController {
        public ActionResult Index() {
            return View();
        }
    }
}