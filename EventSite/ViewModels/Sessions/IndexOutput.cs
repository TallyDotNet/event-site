using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using EventSite.Domain.Queries;

namespace EventSite.ViewModels.Sessions {
    public class IndexOutput {
        public Page<SessionSummaryPage.Summary> Page { get; private set; }
        public string EventSlug { get; private set; }
        public SessionStatus? CurrentStatusFilter { get; private set; }

        public IndexOutput(Page<SessionSummaryPage.Summary> page, string eventSlug, SessionStatus? currentStatusFilter) {
            Page = page;
            EventSlug = eventSlug;
            CurrentStatusFilter = currentStatusFilter;
        }
    }
}