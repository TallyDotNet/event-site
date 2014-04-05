using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using EventSite.Domain;
using EventSite.Domain.Commands;
using EventSite.Domain.Model;
using EventSite.Domain.Queries;
using EventSite.Domain.WorkItems;
using EventSite.Infrastructure;
using EventSite.Infrastructure.Controllers;
using EventSite.Infrastructure.Filters;

namespace EventSite.Controllers {
    public class SponsorsController : BaseController {
        readonly IImageStorage imageStorage;

        public SponsorsController(IImageStorage imageStorage) {
            this.imageStorage = imageStorage;
        }

        [HttpGet]
        public ActionResult Index(string eventSlug = null) {
            if(string.IsNullOrEmpty(eventSlug) && State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            var eventId = string.IsNullOrEmpty(eventSlug)
                ? State.CurrentEvent.Id
                : Event.IdFrom(eventSlug);

            var sponsors = Bus.Query(new SponsorsForEvent(eventId));

            return View(sponsors);
        }

        [HttpGet]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Create(string eventSlug) {
            return View("CreateOrUpdate", new CreateOrUpdateSponsor());
        }

        [HttpPost]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Index(string eventSlug, CreateOrUpdateSponsor input, HttpPostedFileBase file) {
            return Execute(input)
                .OnSuccess(x => {
                    if(file == null || file.ContentLength == 0) {
                        DisplayErrorMessage("You must provide an image.");
                        return View("CreateOrUpdate", input);
                    }

                    x.Subject.ImageSource = imageStorage.Store(file.FileName, file.InputStream);

                    return RedirectToAction("Detail", new {
                        eventSlug,
                        sponsorSlug = Sponsor.SlugFromId(x.Subject.Id)
                    });
                })
                .OnFailure(x => View("CreateOrUpdate", input));
        }

        [HttpGet]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Detail(string eventSlug, string sponsorSlug) {
            var sponsor = DocSession.Load<Sponsor>(Sponsor.IdFrom(eventSlug, sponsorSlug));
            if(sponsor == null) {
                return NotFound();
            }

            return View("CreateOrUpdate", new CreateOrUpdateSponsor {
                Sponsor = sponsor
            });
        }

        [HttpPost]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Detail(string eventSlug, string sponsorSlug, CreateOrUpdateSponsor input, HttpPostedFileBase file = null) {
            input.Sponsor.Id = Sponsor.IdFrom(eventSlug, sponsorSlug);
            return Execute(input)
                .OnSuccess(x => {
                    if(file != null && file.ContentLength > 0) {
                        imageStorage.Remove(x.Subject.ImageSource);
                        x.Subject.ImageSource = imageStorage.Store(file.FileName, file.InputStream);
                    }

                    return RedirectToAction("Detail", new {eventSlug, sponsorSlug});
                })
                .OnFailure(x => View("CreateOrUpdate", input));
        }

        //[HttpGet]
        //[LoggedIn(Roles = Roles.Admin)]
        //public FileResult ExportSponsors(string eventSlug) {
        //    //var eventId = Event.IdFrom(eventSlug);
        //    //var allSponsorsForEvent = getAllSponsors(eventId);

        //    //var downloadFileName = string.Format("attendees_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"));
        //    //var tempFilePath = Path.Combine(Server.MapPath("~/App_Data"), downloadFileName);

        //    //Bus.Do(new ExportAttendeesToExcel(allSponsorsForEvent, new FileInfo(tempFilePath)));

        //    //return File(tempFilePath, "application/xlsx", downloadFileName);
        //}

        //private IEnumerable<Sponsor> getAllSponsors {
        //     var pageNumber = 1;
        //    var sponsors = new List<Attendee>();
        //    Page<Attendee> currentPage;

        //    do {
        //        currentPage = Bus.Query(new AttendeesForEvent(eventId, pageNumber, PageSizeForExport));
        //        attendees.AddRange(currentPage.Items);
        //        pageNumber++;
        //    } while (currentPage.HasNextPage);

        //    return attendees;
        //}
    }
}