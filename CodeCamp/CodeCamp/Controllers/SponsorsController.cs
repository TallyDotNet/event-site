using System.Web.Mvc;
using CodeCamp.Domain;
using CodeCamp.Infrastructure.Controllers;

namespace CodeCamp.Controllers {
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