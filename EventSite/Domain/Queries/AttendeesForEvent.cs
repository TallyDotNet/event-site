using System.Collections.Generic;
using System.Linq;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace EventSite.Domain.Queries
{
    public class AttendeesForEvent : Query<IEnumerable<Attendee>>
    {
        private readonly string eventId ;

        public AttendeesForEvent(string eventId)
        {
            this.eventId = eventId;
        }

        protected override IEnumerable<Attendee> Execute()
        {
            var query = DocSession.Query<Attendee, AttendeesPageIndex>()
                                  .Where(a => a.EventId == eventId)
                                  .AsProjection<Attendee>()
                                  .Include(x => x.User); //pre-load the User so we can get the preferences and determine if they should be included in the listing page

            return query.ToArray()
                        .Select(x =>
                            {
                                x.User = DocSession.Load<User>(x.UserId);
                                return x;
                            });
        }
    }

    public class AttendeesPageIndex : AbstractIndexCreationTask<Registration, Attendee>
    {
        public AttendeesPageIndex()
        {
            Map = registrations =>
                  from registration in registrations
                  let registeredUser = LoadDocument<User>(registration.User.Id)
                  select new
                      {
                          UserId = registration.User.Id,
                          EventId = registration.Event.Id
                      };

            Store(x => x.UserId, FieldStorage.Yes);
            Store(x => x.EventId, FieldStorage.Yes);
        }
    }
}