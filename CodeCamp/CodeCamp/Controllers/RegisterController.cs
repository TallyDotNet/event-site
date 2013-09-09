using System.Web.Mvc;
using CodeCamp.Domain.Commands;
using CodeCamp.Infrastructure.Controllers;
using CodeCamp.Infrastructure.Filters;

namespace CodeCamp.Controllers {
    [LoggedIn]
    public class RegisterController : BaseController {
        [HttpGet]
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public ActionResult Index(RegisterForCurrentEvent input) {
            return Execute(input)
                .Always(x => RedirectToAction("Index", "Account"));
        }
    }
}