using System;
using System.Web.Mvc;
using CodeCamp.Domain.Commands;
using CodeCamp.Domain.Model;
using CodeCamp.Infrastructure.Controllers;
using CodeCamp.Infrastructure.Filters;

namespace CodeCamp.Controllers {
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