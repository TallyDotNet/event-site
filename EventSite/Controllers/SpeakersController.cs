using System.Web.Mvc;
using EventSite.Domain;
using EventSite.Domain.Model;
using EventSite.Domain.Queries;
using EventSite.Infrastructure.Controllers;

namespace EventSite.Controllers {
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