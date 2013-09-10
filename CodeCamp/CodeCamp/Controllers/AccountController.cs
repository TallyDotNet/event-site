using System.Web.Mvc;
using CodeCamp.Domain.Commands;
using CodeCamp.Domain.Model;
using CodeCamp.Domain.Queries;
using CodeCamp.Infrastructure.Controllers;
using CodeCamp.Infrastructure.Filters;
using CodeCamp.ViewModels.Account;

namespace CodeCamp.Controllers {
    public class AccountController : BaseController {
        [HttpGet]
        [LoggedIn]
        public ActionResult Index() {
            var vm = new IndexViewModel();

            if(State.RegistrationStatus == RegistrationStatus.Registered) {
                vm.SubmittedSessions = Bus.Query(new SubmittedSessions());
            }

            return View(vm);
        }

        [HttpPost]
        [LoggedIn]
        public ActionResult Index(UpdateProfile input) {
            return Execute(input).Always(x => View(input));
        }

        [HttpGet]
        public ActionResult Login() {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateAccountViewModel input) {
            return Execute(input)
                .OnSuccess(x => {
                    State.Login(x.Subject, input.Persist);

                    if(State.CurrentEvent != null) {
                        return RedirectToAction("Index", "Register");
                    }

                    return RedirectToAction("Index", "Account");
                })
                .OnFailure(x => View(input));
        }

        [LoggedIn]
        public ActionResult Logout() {
            State.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}