using System.Web.Mvc;
using CodeCamp.Domain;
using CodeCamp.Infrastructure.Controllers;

namespace CodeCamp.Controllers {
    public class LocationController : BaseController {
        [HttpGet]
        public ActionResult Index() {
            if(State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            //TODO: provide venue details

            return View();
        }
    }
}