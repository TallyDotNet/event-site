using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using Raven.Client;
using Raven.Client.Linq;

namespace EventSite.Domain.Queries
{
    public class GetEventStatistics : Query<EventStatistics>
    {
        private readonly string eventId;

        public GetEventStatistics(string eventId)
        {
            this.eventId = eventId;
        }

        protected override EventStatistics Execute()
        {
            RavenQueryStatistics attendeeStats;
            DocSession.Query<Attendee, AttendeesPageIndex>()
                      .Where(a => a.EventId == eventId)
                      .Statistics(out attendeeStats)
                      .AsProjection<Attendee>()
                      .Lazily();

            RavenQueryStatistics sessionStats;
            DocSession.Query<Session, SessionSummaryPage.SessionSummaryPageIndex>()
                      .Where(x => x.Event.Id == eventId)
                      .Statistics(out sessionStats)
                      .Lazily();

            RavenQueryStatistics speakerStats;
            DocSession.Query<Speaker, SpeakersForEvent.SpeakerPageIndex>()
                                         .Where(x => x.EventId == eventId)
                                         .Statistics(out speakerStats)
                                         .Lazily();

            DocSession.Advanced.Eagerly.ExecuteAllPendingLazyOperations();
            return new EventStatistics
                {
                    RegisteredAttendeeCount = attendeeStats.TotalResults,
                    SessionCount = sessionStats.TotalResults,
                    SpeakerCount = speakerStats.TotalResults
                };
        }
    }
}