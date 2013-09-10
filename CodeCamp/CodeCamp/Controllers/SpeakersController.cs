using System.Web.Mvc;
using CodeCamp.Domain;
using CodeCamp.Infrastructure.Controllers;

namespace CodeCamp.Controllers {
    public class SpeakersController : BaseController {
        [HttpGet]
        public ActionResult Index() {
            if(State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            //TODO: paginated speaker list

            return View();
        }
    }
}