using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EventSite.Domain;
using EventSite.Domain.Model;
using EventSite.Domain.Queries;
using EventSite.Infrastructure.Controllers;

namespace EventSite.Controllers {
    public class SpeakersController : BaseController {
        const float ColumnCount = 6;

        [HttpGet]
        public ActionResult Index(string eventSlug = null) {
            if(string.IsNullOrEmpty(eventSlug) && State.NoEventScheduled()) {
                return View("NoEventScheduled");
            }

            var eventId = string.IsNullOrEmpty(eventSlug)
                ? State.CurrentEvent.Id
                : Event.IdFrom(eventSlug);

            var speakers = Bus.Query(new SpeakersForEvent(eventId)).ToArray();
            var rows = new List<List<Speaker>>();

            for(var i = 0; i < speakers.Length; i++) {
                var row = (int) Math.Floor(i/ColumnCount);
                var column = i - (4*row);

                if(column == 0) {
                    rows.Add(new List<Speaker>());
                }

                rows[row].Add(speakers[i]);
            }

            return View(rows);
        }
    }
}