using System;
using System.IO;
using System.Web.Mvc;
using EventSite.Domain.Commands;
using EventSite.Domain.Model;
using EventSite.Domain.Queries;
using EventSite.Domain.WorkItems;
using EventSite.Infrastructure.Controllers;
using EventSite.Infrastructure.Filters;

namespace EventSite.Controllers {
    [LoggedIn(Roles = Roles.Admin)]
    public class EventsController : BaseController {
        private const int PageNumberForExport = 1;
        private const int PageSizeForExport = int.MaxValue;

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
                .OnSuccess(x => RedirectToAction("Detail", new { eventSlug = Event.SlugFromId(x.Subject.Id) }))
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

        [HttpGet]
        public FileResult ExportAttendees(string eventSlug) {

            var eventId = Event.IdFrom(eventSlug);
            var attendees = Bus.Query(new AttendeesForEvent(eventId, PageNumberForExport, PageSizeForExport));

            var downloadFileName = string.Format("attendees_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"));
            var tempFile = Path.Combine(Path.GetTempPath(), downloadFileName);
            Bus.Do(new ExportAttendeesToExcel(attendees.Items, new FileInfo(tempFile)));

            return File(tempFile, "application/xlsx", downloadFileName);

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
                .Always(x => RedirectToAction("Detail", new { eventSlug }));
        }
    }
}