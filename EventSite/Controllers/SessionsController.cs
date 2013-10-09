using System.Web.Mvc;
using EventSite.Domain;
using EventSite.Domain.Commands;
using EventSite.Domain.Model;
using EventSite.Domain.Queries;
using EventSite.Infrastructure.Controllers;
using EventSite.Infrastructure.Filters;
using EventSite.ViewModels.Sessions;

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

            return View(
                new IndexOutput(
                    data,
                    string.IsNullOrEmpty(eventSlug)
                        ? Event.SlugFromId(State.CurrentEvent.Id)
                        : eventSlug,
                    status
                    )
                );
        }

        [HttpPost]
        [LoggedIn]
        public ActionResult Index(string eventSlug, SubmitOrEditSession input) {
            if(State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            if(!State.RegisteredForEvent()) {
                DisplayErrorMessage("Please register before submitting a session.");
                return RedirectToAction("Create", "Registration", new {eventSlug = State.CurrentEventSlug()});
            }

            return Execute(input)
                .OnSuccess(x => {
                    DocSession.SaveChanges();
                    return RedirectToAction("Index", "Account");
                })
                .OnFailure(x => View("CreateOrUpdate", input));
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

            return View("CreateOrUpdate", new SubmitOrEditSession());
        }

        [HttpGet]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Edit(string eventSlug, string sessionSlug) {
            var sessionId = Domain.Model.Session.IdFrom(eventSlug, sessionSlug);
            var session = DocSession.Load<Session>(sessionId);
            if(session == null) {
                return NotFound();
            }

            return View("CreateOrUpdate", new SubmitOrEditSession {
                SessionSlug = sessionSlug,
                EventSlug = eventSlug,
                Description = session.Description,
                Name = session.Name,
                Level = session.Level
            });
        }

        [HttpPost]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Detail(string eventSlug, string sessionSlug, SubmitOrEditSession input) {
            input.EventSlug = eventSlug;
            input.SessionSlug = sessionSlug;

            return Execute(input)
                .OnSuccess(x => RedirectToAction("Index", "Sessions"))
                .OnFailure(x => View("CreateOrUpdate", input));
        }


        [HttpPost]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Approve(string eventSlug, string sessionSlug) {
            return ChangeSessionStatus(eventSlug, sessionSlug, SessionStatus.Approved);
        }

        [HttpPost]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Reject(string eventSlug, string sessionSlug) {
            return ChangeSessionStatus(eventSlug, sessionSlug, SessionStatus.Rejected);
        }

        [HttpPost]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Delete(string eventSlug, string sessionSlug) {
            return ChangeSessionStatus(eventSlug, sessionSlug, SessionStatus.Deleted);
        }

        ActionResult ChangeSessionStatus(string eventSlug, string sessionSlug, SessionStatus newStatus) {
            var sessionId = Domain.Model.Session.IdFrom(eventSlug, sessionSlug);

            return Execute(new ChangeSessionStatus(sessionId, newStatus))
                .Always(x => RedirectToAction("Index"));
        }
    }
}