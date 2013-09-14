using System;
using System.Web.Mvc;
using EventSite.Domain.Commands;
using EventSite.Domain.Model;
using EventSite.Infrastructure.Controllers;
using EventSite.Infrastructure.Filters;

namespace EventSite.Controllers {
    [LoggedIn]
    public class RegistrationController : BaseController {
        [HttpGet]
        public ActionResult Create() {
            switch(State.RegistrationStatus) {
                case RegistrationStatus.NoEventScheduled:
                    return View("NoEventScheduled");
                case RegistrationStatus.NotRegistered:
                    return View();
                case RegistrationStatus.Registered:
                    return View("AlreadyRegistered");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPost]
        public ActionResult Index(RegisterForCurrentEvent input) {
            return Execute(input)
                .Always(x => RedirectToAction("Index", "Account"));
        }
    }
}