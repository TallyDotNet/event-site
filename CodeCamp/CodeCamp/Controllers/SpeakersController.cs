using System.Web.Mvc;
using CodeCamp.Domain;
using CodeCamp.Domain.Queries;
using CodeCamp.Infrastructure.Controllers;

namespace CodeCamp.Controllers {
    public class SpeakersController : BaseController {
        [HttpGet]
        public ActionResult Index(int page = 1) {
            if(State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            var speakers = Bus.Query(new SpeakersForEventPage(State.CurrentEvent.Id, page));

            return View(speakers);
        }
    }
}