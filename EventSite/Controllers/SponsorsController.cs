using System.Web.Mvc;
using EventSite.Domain;
using EventSite.Infrastructure.Controllers;

namespace EventSite.Controllers {
    public class SponsorsController : BaseController {
        [HttpGet]
        public ActionResult Index() {
            if(State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            //TODO: paginated sponsor list

            return View();
        }
    }
}