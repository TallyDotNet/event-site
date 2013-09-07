using System.Web.Mvc;
using CodeCamp.Domain.Commands;
using CodeCamp.Infrastructure.Controllers;

namespace CodeCamp.Controllers {
    public class AccountController : BaseController {
        [HttpGet]
        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateAccount input) {
            return Execute(input)
                .OnSuccess(x => {
                    //log them in
                    return RedirectToAction("", "");
                }).OnFailure(x => {
                    return View("Create", null);
                });
        }
    }
}