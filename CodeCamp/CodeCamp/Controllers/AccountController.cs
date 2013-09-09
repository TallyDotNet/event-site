using System.Web.Mvc;
using CodeCamp.Infrastructure.Controllers;
using CodeCamp.Infrastructure.Filters;
using CodeCamp.ViewModels.Account;

namespace CodeCamp.Controllers {
    public class AccountController : BaseController {
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

                    return RedirectToAction("Profile", "Account");
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