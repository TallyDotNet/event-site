using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using Raven.Client;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace EventSite.Domain.Queries {
    public class SpeakersForEvent : Query<IEnumerable<Speaker>> {
        readonly string eventId;

        public SpeakersForEvent(string eventId) {
            this.eventId = eventId;
        }

        protected override IEnumerable<Speaker> Execute() {
            RavenQueryStatistics statistics;

            var query = DocSession.Query<Speaker, SpeakerPageIndex>()
                .Where(x => x.EventId == eventId)
                .Include(x => x.Id);

            return query.ToArray().Select(x => {
                x.User = DocSession.Load<User>(x.Id);
                return x;
            });
        }

        public class SpeakerPageIndex : AbstractMultiMapIndexCreationTask<Speaker> {
            public SpeakerPageIndex() {
                AddMap<Registration>(
                    registrations =>
                        from reg in registrations
                        where reg.IsSpeaker
                        select new {
                            Id = reg.User.Id,
                            EventId = reg.Event.Id,
                            Sessions = (IEnumerable) null
                        });

                AddMap<Session>(
                    sessions =>
                        from session in sessions
                        where session.Status == SessionStatus.Approved
                        select new {
                            Id = session.User.Id,
                            EventId = session.Event.Id,
                            Sessions = new[] {session}
                        }
                    );

                Reduce = results => from result in results
                    group result by result.EventId + result.Id
                    into g
                    select new {
                        Id = g.First().Id,
                        EventId = g.First().EventId,
                        Sessions = g.SelectMany(x => x.Sessions)
                    };
            }
        }
    }
}