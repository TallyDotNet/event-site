using System.Collections;
using System.Linq;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using Raven.Client;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace EventSite.Domain.Queries {
    public class SpeakersForEvent : Query<Page<Speaker>> {

        private const int PageSize = 24; //these get displayed in rows of 6, so we'll go with 4 rows per page
        readonly string eventId;
        private int page;

        public SpeakersForEvent(string eventId, int page) {
            this.eventId = eventId;
            this.page = page;
        }

        protected override Page<Speaker> Execute() {
            var query = DocSession.Query<Speaker, SpeakerPageIndex>()
                                  .Where(x => x.EventId == eventId)
                                  .Include(x => x.Id)
                                  .OrderBy(x => x.DisplayName);

            RavenQueryStatistics statistics;
            var pagedResults = Page.Transform(query, ref page, out statistics, PageSize)
                                   .ToArray()
                                   .Select(x =>
                                   {
                                       x.User = DocSession.Load<User>(x.Id);
                                       return x;
                                   });

            return new Page<Speaker>
            {
                CurrentPage = page,
                TotalPages = Page.CalculatePages(statistics.TotalResults, PageSize),
                Items = pagedResults
            };
        }

        public class SpeakerPageIndex : AbstractMultiMapIndexCreationTask<Speaker> {
            public SpeakerPageIndex() {
                AddMap<Registration>(
                    registrations =>
                        from reg in registrations
                        let user = LoadDocument<User>(reg.User.Id)
                        where reg.IsSpeaker
                        select new {
                            Id = reg.User.Id,
                            EventId = reg.Event.Id,
                            Sessions = (IEnumerable) null,
                            DisplayName = string.IsNullOrEmpty(user.Profile.Name) ? user.Username : user.Profile.Name
                        });

                AddMap<Session>(
                    sessions =>
                        from session in sessions
                        where session.Status == SessionStatus.Approved
                        let user = LoadDocument<User>(session.User.Id)
                        select new {
                            Id = session.User.Id,
                            EventId = session.Event.Id,
                            Sessions = new[] {session},
                            DisplayName = string.IsNullOrEmpty(user.Profile.Name) ? user.Username : user.Profile.Name
                        }
                    );

                Reduce = results => from result in results
                    group result by result.EventId + result.Id
                    into g
                    select new {
                        Id = g.First().Id,
                        EventId = g.First().EventId,
                        DisplayName = g.First().DisplayName,
                        Sessions = g.SelectMany(x => x.Sessions)
                    };

                
            }
        }
    }
}