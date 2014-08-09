using System;
using System.Collections.Generic;
using System.Linq;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace EventSite.Domain.Queries {
    public class SponsorsForEvent : Query<IEnumerable<Sponsor>> {
        readonly string eventId;
        readonly SponsorStatus[] statuses;

        public SponsorsForEvent(string eventId, params SponsorStatus[] statuses) {
            this.eventId = eventId;
            this.statuses = statuses.Any() ? statuses : new[] {SponsorStatus.Active};
        }

        protected override IEnumerable<Sponsor> Execute() {
            return DocSession.Query<Sponsor, SponsorsIndex>()
                .Where(x => x.Event.Id == eventId)
                .ToList()
                .Where(x => x.Status.In(statuses))
                .OrderBy(x => x.DonatedOn.GetValueOrDefault(DateTimeOffset.Now))
                .OrderByDescending(x => x.Priority)
                .OrderByDescending(x => x.Level);
        }

        public class SponsorsIndex : AbstractIndexCreationTask<Sponsor> {
            public SponsorsIndex() {
                Map = sponsors =>
                    from sponsor in sponsors
                    select new {
                        Event_Id = sponsor.Event.Id
                    };
            }
        }
    }
}