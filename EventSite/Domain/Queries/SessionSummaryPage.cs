using System.Linq;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace EventSite.Domain.Queries {
    public class SessionSummaryPage : Query<Page<SessionSummaryPage.Summary>> {
        readonly string eventId;
        int page;

        public SessionSummaryPage(string eventId, int page) {
            this.eventId = eventId;
            this.page = page;
        }

        protected override Page<Summary> Execute() {
            RavenQueryStatistics statistics;

            var query = DocSession.Query<Session, SessionSummaryPageIndex>()
                .Where(x => x.Event.Id == eventId);

            if(!State.UserIsAdmin()) {
                query = query.Where(x => x.Status == SessionStatus.Approved);
            }

            var paged = Page.Transform(query, ref page, out statistics)
                .AsProjection<Summary>();

            return new Page<Summary> {
                CurrentPage = page,
                TotalPages = statistics.TotalResults/Page.Size,
                Items = paged.ToArray()
            };
        }

        public class Summary {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string SubmitterId { get; set; }
            public string SubmitterName { get; set; }
            public string SubmitterEmail { get; set; }
            public SessionStatus Status { get; set; }
            public string Event_Id { get; set; }
        }

        public class SessionSummaryPageIndex : AbstractIndexCreationTask<Session, Summary> {
            public SessionSummaryPageIndex() {
                Map = sessions =>
                    from session in sessions
                    let submitter = LoadDocument<User>(session.User.Id)
                    select new Summary {
                        Id = session.Id,
                        Name = session.Name,
                        Description = session.Description,
                        SubmitterId = session.User.Id,
                        SubmitterName = string.IsNullOrEmpty(submitter.Profile.Name) ? submitter.Username : submitter.Profile.Name,
                        SubmitterEmail = submitter.Email,
                        Status = session.Status,
                        Event_Id = session.Event.Id
                    };

                Store(x => x.SubmitterId, FieldStorage.Yes);
                Store(x => x.SubmitterName, FieldStorage.Yes);
                Store(x => x.SubmitterEmail, FieldStorage.Yes);
                Store(x => x.Event_Id, FieldStorage.Yes);
            }
        }
    }
}