using System.Web.Mvc;
using CodeCamp.Domain.Commands;
using CodeCamp.Domain.Model;
using CodeCamp.Infrastructure.Controllers;
using CodeCamp.Infrastructure.Filters;

namespace CodeCamp.Controllers {
    [LoggedIn(Roles = Roles.Admin)]
    public class EventsController : BaseController {
        public ActionResult Index() {
            var events = DocSession.Query<Event>();
            return View(events);
        }

        [HttpGet]
        public ActionResult Create() {
            return View("CreateOrUpdate", new CreateOrUpdateEvent());
        }

        [HttpPost]
        public ActionResult Create(CreateOrUpdateEvent input) {
            return Execute(input)
                .OnSuccess(x => RedirectToAction("Edit", new { slug = Event.SlugFromId(x.Subject.Id) }))
                .OnFailure(x => View("CreateOrUpdate", input));
        }

        [HttpGet]
        public ActionResult Edit(string slug) {
            var ev = DocSession.Load<Event>(Event.IdFrom(slug));
            if(ev == null) {
                return NotFound();
            }

            return View("CreateOrUpdate", new CreateOrUpdateEvent {
                Slug = slug,
                Event = ev
            });
        }

        [HttpPost]
        public ActionResult Edit(string slug, CreateOrUpdateEvent input) {
            input.Event.Id = Event.IdFrom(slug);
            return Execute(input)
                .OnSuccess(x => RedirectToAction("Edit", new {slug}))
                .OnFailure(x => View("CreateOrUpdate", input));
        }

        [HttpPost]
        public ActionResult MakeCurrent(string slug) {
            return Execute(new MakeEventCurrent(Event.IdFrom(slug)))
                .Always(x => RedirectToAction("Edit", new { slug }));
        }
    }
}