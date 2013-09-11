using System.Linq;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace CodeCamp.Domain.Queries {
    public class SessionSummaryPage : Query<Page<SessionSummaryPage.Summary>> {
        const int PageSize = 25;
        int page;

        public SessionSummaryPage(int page) {
            this.page = page;
        }

        protected override Page<Summary> Execute() {
            if(page < 1) {
                page = 1;
            }

            var pageIndex = page - 1;
            RavenQueryStatistics statistics;

            var query = DocSession.Query<Session, SessionSummaryPageIndex>();

            if(!State.UserIsAdmin()) {
                query = query.Where(x => x.Status == SessionStatus.Approved);
            }

            query.Statistics(out statistics);

            var paged = query.Skip(pageIndex*PageSize)
                .Take(PageSize)
                .AsProjection<Summary>();

            return new Page<Summary> {
                CurrentPage = page,
                TotalPages = statistics.TotalResults/PageSize,
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
        }

        public class SessionSummaryPageIndex : AbstractIndexCreationTask<Session, Summary> {
            public SessionSummaryPageIndex() {
                Map = sessions =>
                    from session in sessions
                    let submitter = LoadDocument<User>(session.Submitter.Id)
                    select new Summary {
                        Id = session.Id,
                        Name = session.Name,
                        Description = session.Description,
                        SubmitterId = session.Submitter.Id,
                        SubmitterName = string.IsNullOrEmpty(submitter.Profile.Name) ? submitter.Username : submitter.Profile.Name,
                        SubmitterEmail = submitter.Email,
                        Status = session.Status
                    };

                Store(x => x.SubmitterId, FieldStorage.Yes);
                Store(x => x.SubmitterName, FieldStorage.Yes);
                Store(x => x.SubmitterEmail, FieldStorage.Yes);
            }
        }
    }
}