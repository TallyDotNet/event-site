using System.Collections;
using System.Linq;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using Raven.Client;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace EventSite.Domain.Queries {
    public class SpeakersForEventPage : Query<Page<Speaker>> {
        readonly string eventId;
        int page;

        public SpeakersForEventPage(string eventId, int page) {
            this.eventId = eventId;
            this.page = page;
        }

        protected override Page<Speaker> Execute() {
            RavenQueryStatistics statistics;

            var query = DocSession.Query<Speaker, SpeakerPageIndex>()
                .Where(x => x.EventId == eventId)
                .Include(x => x.Id);

            var paged = Page.Transform(query, ref page, out statistics);

            return new Page<Speaker> {
                CurrentPage = page,
                TotalPages = statistics.TotalResults/Page.Size,
                Items = paged.ToArray().Select(x => {
                    x.User = DocSession.Load<User>(x.Id);
                    return x;
                })
            };
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
                            Sessions = (IEnumerable)null
                        });

                AddMap<Session>(
                    sessions => 
                        from session in sessions
                        where session.Status == SessionStatus.Approved
                        select new {
                            Id = session.User.Id,
                            EventId = session.Event.Id,
                            Sessions = new[]{ session }
                        }
                    );

                Reduce = results => from result in results
                    group result by result.EventId
                    into g
                    select new {
                        Id = g.First().Id,
                        EventId = g.Key,
                        Sessions = g.SelectMany(x => x.Sessions)
                    };
            }
        }
    }
}