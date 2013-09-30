using System.Collections.Generic;
using System.Linq;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using Raven.Client.Indexes;

namespace EventSite.Domain.Queries {
    public class SponsorsForEvent : Query<IEnumerable<Sponsor>> {
        readonly string eventId;

        public SponsorsForEvent(string eventId) {
            this.eventId = eventId;
        }

        protected override IEnumerable<Sponsor> Execute() {
            return DocSession.Query<Sponsor, SponsorsIndex>()
                .Where(x => x.Event.Id == eventId)
                .OrderByDescending(x => x.Priority)
                .ToList()
                .OrderByDescending(x => x.Level);
        }

        public class SponsorsIndex : AbstractIndexCreationTask<Sponsor> {
            public SponsorsIndex() {
                Map = sponsors =>
                    from sponsor in sponsors
                    select new {
                        Event_Id = sponsor.Event.Id,
                        sponsor.Priority
                    };
            }
        }
    }
}