using System.Web.Mvc;
using CodeCamp.Domain.Model;
using CodeCamp.Infrastructure.Controllers;
using CodeCamp.Infrastructure.Filters;

namespace CodeCamp.Controllers {
    [LoggedIn(Roles = Roles.Admin)]
    public class AdminController : BaseController {
        public ActionResult Index() {
            return View();
        }
    }
}