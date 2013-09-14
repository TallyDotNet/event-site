using System.Web.Mvc;
using CodeCamp.Domain.Commands;
using CodeCamp.Domain.Model;
using CodeCamp.Infrastructure.Controllers;
using CodeCamp.Infrastructure.Filters;

namespace CodeCamp.Controllers {
    [LoggedIn(Roles = Roles.Admin)]
    public class EventsController : BaseController {
        [HttpGet]
        public ActionResult Index() {
            var events = DocSession.Query<Event>();
            return View(events);
        }

        [HttpGet]
        public ActionResult Create() {
            return View("CreateOrUpdate", new CreateOrUpdateEvent());
        }

        [HttpPost]
        public ActionResult Index(CreateOrUpdateEvent input) {
            return Execute(input)
                .OnSuccess(x => RedirectToAction("Detail", new {eventSlug = Event.SlugFromId(x.Subject.Id)}))
                .OnFailure(x => View("CreateOrUpdate", input));
        }

        [HttpGet]
        public ActionResult Detail(string eventSlug) {
            var ev = DocSession.Load<Event>(Event.IdFrom(eventSlug));
            if(ev == null) {
                return NotFound();
            }

            return View("CreateOrUpdate", new CreateOrUpdateEvent {
                Slug = eventSlug,
                Event = ev
            });
        }

        [HttpPost]
        public ActionResult Detail(string eventSlug, CreateOrUpdateEvent input) {
            input.Event.Id = Event.IdFrom(eventSlug);
            return Execute(input)
                .OnSuccess(x => RedirectToAction("Detail", new {eventSlug}))
                .OnFailure(x => View("CreateOrUpdate", input));
        }

        [HttpPost]
        public ActionResult MakeCurrent(string eventSlug) {
            return Execute(new MakeEventCurrent(Event.IdFrom(eventSlug)))
                .Always(x => RedirectToAction("Detail", new {eventSlug}));
        }
    }
}