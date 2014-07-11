using System.Resources;
using System.Web.Mvc;
using EventSite.Domain;
using EventSite.Domain.Commands;
using EventSite.Domain.Queries;
using EventSite.Infrastructure.Controllers;
using EventSite.Infrastructure.Filters;
using EventSite.Infrastructure.Routing;
using EventSite.ViewModels.Account;

namespace EventSite.Controllers {
    public class AccountController : BaseController {
        [HttpGet]
        [LoggedIn]
        public ActionResult Index() {
            var vm = new IndexViewModel();

            if(State.RegisteredForEvent()) {
                vm.SubmittedSessions = Bus.Query(new SubmittedSessions());
            }

            return View(vm.WithUser(CurrentUser));
        }

        [HttpPost]
        [LoggedIn]
        public ActionResult Index(IndexViewModel input) {
            return Execute(input)
                .OnSuccess(x => RedirectToAction("Index"))
                .OnFailure(x => {
                    if(State.RegisteredForEvent()) {
                        input.SubmittedSessions = Bus.Query(new SubmittedSessions());
                    }

                    return View(input);
                });
        }

        [HttpGet]
        public ActionResult Login() {

            var rootAuthUrl = GetRootUrl();
            var loginView = new LoginViewModel();
            loginView.GoogleAuthUrl = rootAuthUrl + "google";
            loginView.FacebookAuthUrl = rootAuthUrl + "facebook";
            loginView.LinkedInAuthUrl = rootAuthUrl + "linkedin";
            loginView.TwitterAuthUrl = rootAuthUrl + "twitter";
            loginView.GithubAuthUrl = rootAuthUrl + "github";
            loginView.WindowLiveAuthUrl = rootAuthUrl + "windowslive";

            return View(loginView);
        }

        private string GetRootUrl() {
            return ShouldUseActualAuthUrls() ? "/authentication/redirect/" : "/authentication/redirect/fake";
        }

        private bool ShouldUseActualAuthUrls() {
            return State.Settings.AuthenticationMode == AuthenticationModes.Actual ||
                   (State.Settings.AuthenticationMode == AuthenticationModes.Default && State.RunningInProduction());
        }

        [HttpPost]
        public ActionResult Create(CreateAccountViewModel input) {
            return Execute(input)
                .OnSuccess(x => {
                    var returnUrl = RouteHelper.GetReturnUrl(input.ReturnUrl);
                    State.Login(x.Subject, input.Persist);

                    if(State.EventScheduled()) {
                        return Execute(new RegisterForCurrentEvent())
                            .OnSuccess(y => {
                                if(!string.IsNullOrEmpty(returnUrl)) {
                                    return Redirect(returnUrl);
                                }

                                return RedirectToAction("Index", "Account");
                            }).OnFailure(y => RedirectToAction("Index", "Account"));
                    }

                    if(!string.IsNullOrEmpty(returnUrl)) {
                        return Redirect(returnUrl);
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