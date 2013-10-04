using System.Linq;
using System.Web.Mvc;
using EventSite.Domain;
using EventSite.Domain.Model;
using EventSite.Domain.Queries;
using EventSite.ViewModels.RegisteredUsers;
using EventSite.Infrastructure.Controllers;

namespace EventSite.Controllers
{
    public class AttendeesController : BaseController
    {
        public ActionResult Index(string eventSlug = null)
        {
            if (string.IsNullOrEmpty(eventSlug) && State.NoEventScheduled())
            {
                return View("NoEventScheduled");
            }

            var eventId = string.IsNullOrEmpty(eventSlug)
                ? State.CurrentEvent.Id
                : Event.IdFrom(eventSlug);

            var speakers = Bus.Query(new AttendeesForEvent(eventId)).ToList();

            return View(new IndexOutput<Attendee>(speakers, null));
        }
    }
}
