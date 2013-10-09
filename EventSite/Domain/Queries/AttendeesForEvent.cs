﻿using System.Linq;
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
        private const int PageSize = 24; //these get displayed in rows of 6, so we'll go with 4 rows per page
        readonly string eventId;
        private int page;

        public AttendeesForEvent(string eventId, int page)
        {
            this.eventId = eventId;
            this.page = page;
        }

        protected override Page<Attendee> Execute()
        {
            var query = DocSession.Query<Attendee, AttendeesPageIndex>()
                                  .Where(a => a.EventId == eventId)
                                  .OrderBy(x => x.DisplayName);

            if (!State.UserIsAdmin())
                query = query.Where(x => x.ListInDirectory);

            RavenQueryStatistics statistics;
            var pagedResults = Page.Transform(query, ref page, out statistics, PageSize)
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
                    TotalPages = Page.CalculatePages(statistics.TotalResults, PageSize),
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