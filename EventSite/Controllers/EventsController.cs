using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using EventSite.Domain.Commands;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using EventSite.Domain.Queries;
using EventSite.Domain.WorkItems;
using EventSite.Infrastructure.Controllers;
using EventSite.Infrastructure.Data.Export;
using EventSite.Infrastructure.Filters;
using EventSite.Infrastructure.Helpers;

namespace EventSite.Controllers {
    [LoggedIn(Roles = Roles.Admin)]
    public class EventsController : BaseController {
        const int PageSizeForExport = 50;

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
            return PerformExcelExport("attendees",
                new AttendeesForEvent(eventId, 1, PageSizeForExport),
                new AttendeesExportColumnMappings());
        }

        [HttpGet]
        public FileResult ExportSponsors(string eventSlug) {
            var eventId = Event.IdFrom(eventSlug);
            return PerformExcelExport("sponsors",
                new SponsorsForEvent(eventId),
                new SponsorsExportColumnMappings());
        }

        private FileResult PerformExcelExport<T>(string rootFileName, Query<Page<T>> query, IExportColumnMappings<T> columns) {
            var exportItems = GetExcelExportItems(rootFileName, columns);
            exportItems.FileBuilder.Build(query, exportItems.Exporter);
            return File(exportItems.TempFileInfo.FullName, "application/xlsx", exportItems.TempFileInfo.Name);
        }

        private FileResult PerformExcelExport<T>(string rootFileName, Query<IEnumerable<T>> query, IExportColumnMappings<T> columns) {
            var exportItems = GetExcelExportItems(rootFileName, columns);
            exportItems.FileBuilder.Build(query, exportItems.Exporter);
            return File(exportItems.TempFileInfo.FullName, "application/xlsx", exportItems.TempFileInfo.Name);
        }

        private dynamic GetExcelExportItems<T>(string rootFileName, IExportColumnMappings<T> columns) {
            var tempFileInfo = BuildExcelExportTempPath(rootFileName);
            return new
            {
                TempFileInfo = tempFileInfo,
                Exporter = new ExcelExporter<T>(columns, new FileInfoTargetFile(tempFileInfo), Bus),
                FileBuilder = new ExcelFileBuilder(Bus)
            };
        }

        private FileInfo BuildExcelExportTempPath(string rootFileName) {
            var timeStampedFileName = string.Format("{0}_{1}.xlsx", rootFileName, DateTime.Now.ToString("yyyyMMddHHmmss"));
            return new FileInfo(Path.Combine(Server.MapPath("~/App_Data"), timeStampedFileName));
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