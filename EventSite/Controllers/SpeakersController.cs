using System.Linq;
using System.Web.Mvc;
using EventSite.Domain;
using EventSite.Domain.Model;
using EventSite.Domain.Queries;
using EventSite.Infrastructure.Controllers;
using EventSite.ViewModels.Speakers;

namespace EventSite.Controllers {
    public class SpeakersController : BaseController {
        [HttpGet]
        public ActionResult Index(string eventSlug = null, string speakerSlug = null) {
            if(string.IsNullOrEmpty(eventSlug) && State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            var eventId = string.IsNullOrEmpty(eventSlug)
                ? State.CurrentEvent.Id
                : Event.IdFrom(eventSlug);

            var speakers = Bus.Query(new SpeakersForEvent(eventId)).ToList();

            return View(new IndexOutput(speakers, speakerSlug));
        }
    }
}