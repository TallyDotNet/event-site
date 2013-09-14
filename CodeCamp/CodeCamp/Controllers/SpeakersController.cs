using System.Web.Mvc;
using CodeCamp.Domain;
using CodeCamp.Domain.Model;
using CodeCamp.Domain.Queries;
using CodeCamp.Infrastructure.Controllers;

namespace CodeCamp.Controllers {
    public class SpeakersController : BaseController {
        [HttpGet]
        public ActionResult Index(string eventSlug = null, int page = 1) {
            if(string.IsNullOrEmpty(eventSlug) && State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            var eventId = string.IsNullOrEmpty(eventSlug)
                ? State.CurrentEvent.Id
                : Event.IdFrom(eventSlug);

            var speakers = Bus.Query(new SpeakersForEventPage(eventId, page));

            return View(speakers);
        }
    }
}