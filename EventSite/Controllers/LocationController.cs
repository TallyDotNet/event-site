using System.Web.Mvc;
using EventSite.Domain;
using EventSite.Domain.Model;
using EventSite.Infrastructure.Controllers;

namespace EventSite.Controllers {
    public class LocationController : BaseController {
        [HttpGet]
        public ActionResult Index(string eventSlug) {
            if(string.IsNullOrEmpty(eventSlug) && State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            var ev = string.IsNullOrEmpty(eventSlug)
                ? State.CurrentEvent
                : DocSession.Load<Event>(Event.IdFrom(eventSlug));

            return View(ev);
        }
    }
}