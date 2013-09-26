using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using EventSite.Domain.Queries;
using EventSite.Infrastructure.Controllers;

namespace EventSite.Controllers {
    public class HomeController : BaseController {
        public ActionResult Index() {
            var attendees = Bus.Query(new AttendeesForEvent(State.CurrentEvent.Id));
            Debug.Print(attendees.ToList().Count.ToString());
            return View();

            

        }
    }
}