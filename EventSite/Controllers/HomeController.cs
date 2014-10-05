using System.Web.Mvc;
using EventSite.Domain;
using EventSite.Domain.Model;
using EventSite.Domain.Queries;
using EventSite.Infrastructure.Controllers;

namespace EventSite.Controllers {
    public class HomeController : BaseController {
        public ActionResult Index() {
            var stats = State.EventScheduled()
                ? Bus.Query(new GetEventStatistics(State.CurrentEvent.Id))
                : new EventStatistics();

            return View(stats);
        }

        public ActionResult Contact() {
            return View();
        }
    }
}