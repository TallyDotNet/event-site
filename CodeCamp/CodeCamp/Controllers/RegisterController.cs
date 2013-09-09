using System.Web.Mvc;
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
        public ActionResult Current() {
            //TODO: redirect to update profile
            return View();
        }
    }
}