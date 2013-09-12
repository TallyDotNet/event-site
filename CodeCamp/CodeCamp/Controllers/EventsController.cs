using System.Web.Mvc;
using CodeCamp.Infrastructure.Controllers;

namespace CodeCamp.Controllers {
    public class EventsController : BaseController {
        public ActionResult Index() {
            return View();
        }
    }
}