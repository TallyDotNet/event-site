using System.Collections.Generic;
using System.Linq;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace EventSite.Domain.Queries {
    public class AttendeesForEvent : Query<IEnumerable<Attendee>> {
        readonly string eventId;

        public AttendeesForEvent(string eventId) {
            this.eventId = eventId;
        }

        protected override IEnumerable<Attendee> Execute() {
            var query = DocSession.Query<Attendee, AttendeesPageIndex>()
                .Where(a => a.EventId == eventId && a.ListInDirectory);

            return query.AsProjection<Attendee>().ToArray()
                        .Select(x =>
                            {
                                x.User = DocSession.Load<User>(x.UserId);
                                return x;
                            });
        }
    }

    public class AttendeesPageIndex : AbstractIndexCreationTask<Registration, Attendee> {
        public AttendeesPageIndex() {
            Map = registrations =>
                from registration in registrations
                let registeredUser = LoadDocument<User>(registration.User.Id)
                select new Attendee {
                    EventId = registration.Event.Id, 
                    UserId = registration.User.Id,
                    ListInDirectory = registeredUser.Preferences.ListInAttendeeDirectory
                };

            Store(x => x.UserId, FieldStorage.Yes);
            //Store(x => x.User, FieldStorage.Yes);
        }
    }
}