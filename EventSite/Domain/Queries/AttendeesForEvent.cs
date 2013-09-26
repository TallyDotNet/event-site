using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
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
                                  .Include(a => a.User.Id); //not sure of the purpose of this.

            return query.AsProjection<Attendee>().ToList();

            //return query.ToArray().Select(a =>
            //    {
            //        //could these details be included in the map below and "flattened" into the Attendee class in the index?
            //        a.User = DocSession.Load<User>(a.Id);
            //        return a;
            //    });
        }
    }

    public class AttendeesPageIndex : AbstractIndexCreationTask<Registration, Attendee>
    {
        public AttendeesPageIndex()
        {
            Map = registrations =>
                  from registration in registrations
                  select new Attendee
                      {
                          Id = registration.Event.Id + registration.User.Id,
                          EventId = registration.Event.Id,
                      };
        }
    }

    //public class AttendeePageIndex : AbstractMultiMapIndexCreationTask<Attendee>
    //{
    //    public AttendeePageIndex()
    //    {
    //        AddMap<Registration>(
    //            registrations =>
    //            from reg in registrations
    //            select new
    //                {
    //                    Id = reg.User.Id,
    //                    EventId = reg.Event.Id
    //                });

    //        AddMap<User>(
    //            users =>
    //            from user in users
    //            where user.Preferences.ListInAttendeeDirectory
    //            select new
    //                {
    //                    Id = user.Id,
    //                    EventId = 
    //                });

    //        Reduce = results => from result in results
    //                            group result by result.EventId + result.Id
    //                            into g
    //                            select new
    //                                {
    //                                    Id = g.First().Id,
    //                                    EventId = g.First().EventId
    //                                };
    //    }
    //}
}