using System.Web.Mvc;
using EventSite.Domain.Commands;
using EventSite.Domain.Model;
using EventSite.Infrastructure.Controllers;
using EventSite.Infrastructure.Filters;
using Microsoft.Ajax.Utilities;

namespace EventSite.Controllers
{
    public class UsersController : BaseController {
        
        [HttpGet]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Update(string userSlug) {

            var user = LoadUserFromSlug(userSlug);
            if (user == null) {
                return NotFound();
            }

            var userData = new UpdateUser();
            userData.User = user;
            userData.InAdminRole = user.InRole(Roles.Admin);
            userData.InSponsorManagerRole = user.InRole(Roles.ManageSponsors);

            return View(userData);
        }

        [HttpPost]
        [LoggedIn(Roles = Roles.Admin)]
        public ActionResult Update(string userSlug, UpdateUser command) {

            command.User.Id = EventSite.Domain.Model.User.IdFrom(userSlug);

            return Execute(command)
                .OnSuccess(x => View("Update", command))
                .OnFailure(x => View("Update", command));
        }

        private User LoadUserFromSlug(string userSlug) {
            if (userSlug.IsNullOrWhiteSpace()) {
                return null;
            }

            var userId = EventSite.Domain.Model.User.IdFrom(userSlug);
            return DocSession.Load<User>(userId);
        }
    }
}
