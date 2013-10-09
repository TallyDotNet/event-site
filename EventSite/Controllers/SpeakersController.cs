using System.Web.Mvc;
using EventSite.Domain;
using EventSite.Domain.Model;
using EventSite.Domain.Queries;
using EventSite.Infrastructure.Controllers;
using EventSite.ViewModels.RegisteredUsers;

namespace EventSite.Controllers {
    public class SpeakersController : BaseController {
        [HttpGet]
        public ActionResult Index(string eventSlug = null, string speakerSlug = null, int page = 1) {
            if(string.IsNullOrEmpty(eventSlug) && State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            var eventId = string.IsNullOrEmpty(eventSlug)
                ? State.CurrentEvent.Id
                : Event.IdFrom(eventSlug);

            var speakers = Bus.Query(new SpeakersForEvent(eventId, page));

            return View(new IndexOutput<Speaker>(speakers, speakerSlug, eventSlug));
        }
    }
}