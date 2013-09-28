using System.Web.Mvc;
using EventSite.Domain;
using EventSite.Domain.Commands;
using EventSite.Domain.Model;
using EventSite.Infrastructure.Controllers;
using EventSite.Infrastructure.Filters;

namespace EventSite.Controllers {
    public class SponsorsController : BaseController {
        [HttpGet]
        public ActionResult Index() {
            if(State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            //TODO: paginated sponsor list

            return View();
        }

        [HttpGet]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Create() {
            return View("CreateOrUpdate", new CreateOrUpdateSponsor());
        }

        [HttpPost]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Index(CreateOrUpdateSponsor input) {
            return Execute(input)
                .OnSuccess(x => RedirectToAction("Detail", new {
                    eventSlug = Event.SlugFromId(x.Subject.Event.Id),
                    sponsorSlug = Sponsor.SlugFromId(x.Subject.Id)
                }))
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
        public ActionResult Detail(string eventSlug, string sponsorSlug, CreateOrUpdateSponsor input) {
            return Execute(input)
                .OnSuccess(x => RedirectToAction("Detail", new {eventSlug, sponsorSlug}))
                .OnFailure(x => View("CreateOrUpdate", input));
        }
    }
}