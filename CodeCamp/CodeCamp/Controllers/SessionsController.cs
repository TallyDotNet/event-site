using System.Web.Mvc;
using CodeCamp.Domain;
using CodeCamp.Domain.Commands;
using CodeCamp.Infrastructure.Controllers;
using CodeCamp.Infrastructure.Filters;

namespace CodeCamp.Controllers {
    public class SessionsController : BaseController {
        [HttpGet]
        public ActionResult Index() {
            if(State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            //TODO: paginated events list

            return View();
        }

        [HttpGet]
        [LoggedIn]
        public ActionResult Create() {
            if(State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            return View(new SubmitSession());
        }

        [HttpPost]
        [LoggedIn]
        public ActionResult Create(SubmitSession input) {
            return Execute(input)
                .OnSuccess(x => {
                    DocSession.SaveChanges();
                    return RedirectToAction("Index", "Account");
                })
                .OnFailure(x => View(input));
        }
    }
}