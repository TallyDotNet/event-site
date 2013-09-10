using System.Web.Mvc;
using CodeCamp.Infrastructure.Controllers;

namespace CodeCamp.Controllers {
    public class SessionsController : BaseController {
        public ActionResult Index() {
            return View();
        }

        public ActionResult Create() {
            return View();
        }
    }
}