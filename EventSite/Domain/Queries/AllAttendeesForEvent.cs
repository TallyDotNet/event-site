using System.Collections.Generic;
using System.Linq;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using Raven.Client;
using Raven.Client.Linq;

namespace EventSite.Domain.Queries {
    public class AllAttendeesForEvent : Query<IEnumerable<Attendee>> {
        private readonly string eventId;

        public AllAttendeesForEvent(string eventId) {
            this.eventId = eventId;
        }

        protected override IEnumerable<Attendee> Execute() {
            var query = DocSession.Query<Attendee, AttendeesPageIndex>()
                                  .Where(a => a.EventId == eventId)
                                  .OrderBy(x => x.DisplayName);

            if (!State.UserIsAdmin())
                query = query.Where(x => x.ListInDirectory);

            return query.AsProjection<Attendee>().ToArray();
        }
    }
}