using System.Web.Mvc;
using CodeCamp.Domain;
using CodeCamp.Domain.Commands;
using CodeCamp.Domain.Model;
using CodeCamp.Domain.Queries;
using CodeCamp.Infrastructure.Controllers;
using CodeCamp.Infrastructure.Filters;

namespace CodeCamp.Controllers {
    public class SessionsController : BaseController {
        [HttpGet]
        public ActionResult Index(int page = 1) {
            if(State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            var data = Bus.Query(new SessionSummaryPage(State.CurrentEvent.Id, page));

            return View(data);
        }

        [HttpGet]
        [LoggedIn]
        public ActionResult Submit() {
            if(State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            return View(new SubmitSession());
        }

        [HttpPost]
        [LoggedIn]
        public ActionResult Submit(SubmitSession input) {
            return Execute(input)
                .OnSuccess(x => {
                    DocSession.SaveChanges();
                    return RedirectToAction("Index", "Account");
                })
                .OnFailure(x => View(input));
        }

        [HttpPost]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Approve(string sessionId) {
            if(State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            return Execute(new ChangeSessionStatus(sessionId, SessionStatus.Approved))
                .Always(x => RedirectToAction("Index"));
        }

        [HttpPost]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Reject(string sessionId) {
            if(State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            return Execute(new ChangeSessionStatus(sessionId, SessionStatus.Rejected))
                .Always(x => RedirectToAction("Index"));
        }
    }
}