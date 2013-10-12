using System.Linq;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace EventSite.Domain.Queries
{
    public class AttendeesForEvent : Query<Page<Attendee>>
    {
        readonly string eventId;
        readonly int pageSize;
        int page;

        public AttendeesForEvent(string eventId, int page, int pageSize)
        {
            this.eventId = eventId;
            this.page = page;
            this.pageSize = pageSize;
        }

        protected override Page<Attendee> Execute()
        {
            var query = DocSession.Query<Attendee, AttendeesPageIndex>()
                                  .Where(a => a.EventId == eventId)
                                  .OrderBy(x => x.DisplayName);

            if (!State.UserIsAdmin())
                query = query.Where(x => x.ListInDirectory);

            RavenQueryStatistics statistics;
            var pagedResults = Page.Transform(query, ref page, out statistics, pageSize)
                                   .AsProjection<Attendee>()
                                   .ToArray()
                                   .Select(x =>
                                       {
                                           x.User = DocSession.Load<User>(x.User.Id);
                                           return x;
                                       });
            

            return new Page<Attendee>
                {
                    CurrentPage = page,
                    TotalPages = Page.CalculatePages(statistics.TotalResults, pageSize),
                    Items = pagedResults
                }; 
        }
    }

    public class AttendeesPageIndex : AbstractIndexCreationTask<Registration, Attendee>
    {
        public AttendeesPageIndex() 
        {
            Map = registrations =>
                from registration in registrations
                let registeredUser = LoadDocument<User>(registration.User.Id)
                select new Attendee
                {
                    EventId = registration.Event.Id,
                    UserId = registration.User.Id,
                    ListInDirectory = registeredUser.Preferences.ListInAttendeeDirectory,
                    DisplayName = string.IsNullOrEmpty(registeredUser.Profile.Name) ? registeredUser.Username : registeredUser.Profile.Name
                };

            Store(x => x.UserId, FieldStorage.Yes);
            Store(x => x.EventId, FieldStorage.Yes);
            Store(x => x.ListInDirectory, FieldStorage.Yes);
            Store(x => x.DisplayName, FieldStorage.Yes);
        }
    }
}