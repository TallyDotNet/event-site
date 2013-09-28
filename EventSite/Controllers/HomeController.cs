using System.Web.Mvc;
using EventSite.Domain.Queries;
using EventSite.Infrastructure.Controllers;

namespace EventSite.Controllers {
    public class HomeController : BaseController {
        public ActionResult Index() {
            var stats = Bus.Query(new GetEventStatistics(State.CurrentEvent.Id));

            ViewBag.TotalAttendees = stats.RegisteredAttendeeCount;
            ViewBag.TotalSessions = stats.SessionCount;
            ViewBag.TotalSpeakers = stats.SpeakerCount;
            return View();
        }
    }
}