using System.Web.Mvc;
using System.Web.Services.Description;
using EventSite.Domain;
using EventSite.Domain.Commands;
using EventSite.Domain.Model;
using EventSite.Domain.Queries;
using EventSite.Infrastructure.Controllers;
using EventSite.Infrastructure.Filters;

namespace EventSite.Controllers {
    public class SessionsController : BaseController {
        [HttpGet]
        public ActionResult Index(string eventSlug = null, int page = 1, SessionStatus? status = null) {
            if(string.IsNullOrEmpty(eventSlug) && State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            var eventId = string.IsNullOrEmpty(eventSlug)
                ? State.CurrentEvent.Id
                : Event.IdFrom(eventSlug);

            var data = Bus.Query(new SessionSummaryPage(eventId, page, status));
            ViewData["currentEventSlug"] = eventSlug;
            //ViewData["currentStatusFilter"] = status.HasValue ? status.Value.ToString("g") : string.Empty;
            return View(data);
        }

        [HttpGet]
        [LoggedIn]
        public ActionResult Create() {
            if(State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            if(!State.RegisteredForEvent()) {
                DisplayErrorMessage("Please register before submitting a session.");
                return RedirectToAction("Create", "Registration", new {eventSlug = State.CurrentEventSlug()});
            }

            return View(new SubmitSession());
        }

        [HttpPost]
        [LoggedIn]
        public ActionResult Index(string eventSlug, SubmitSession input) {
            if(State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            if(!State.RegisteredForEvent()) {
                DisplayErrorMessage("Please register before submitting a session.");
                return RedirectToAction("Create", "Registration", new{eventSlug=State.CurrentEventSlug()});
            }

            return Execute(input)
                .OnSuccess(x => {
                    DocSession.SaveChanges();
                    return RedirectToAction("Index", "Account");
                })
                .OnFailure(x => View("Create", input));
        }

        [HttpPost]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Approve(string eventSlug, string sessionSlug) {
            var sessionId = Domain.Model.Session.IdFrom(eventSlug, sessionSlug);

            return Execute(new ChangeSessionStatus(sessionId, SessionStatus.Approved))
                .Always(x => RedirectToAction("Index"));
        }

        [HttpPost]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Reject(string eventSlug, string sessionSlug) {
            var sessionId = Domain.Model.Session.IdFrom(eventSlug, sessionSlug);

            return Execute(new ChangeSessionStatus(sessionId, SessionStatus.Rejected))
                .Always(x => RedirectToAction("Index"));
        }
    }
}