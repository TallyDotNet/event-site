using System.Collections.Generic;
using System.Linq;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;
using Raven.Client.Indexes;

namespace EventSite.Domain.Queries {
    public class SubmittedSessions : Query<IEnumerable<Session>> {
        string eventId;
        string userId;

        public SubmittedSessions(string eventId = null, string userId = null) {
            this.eventId = eventId;
            this.userId = userId;
        }

        protected override IEnumerable<Session> Execute() {
            eventId = eventId ?? (State.EventScheduled() ? State.CurrentEvent.Id : null);
            userId = userId ?? (State.UserIsLoggedIn() ? State.User.Id : null);

            if(string.IsNullOrEmpty(eventId) || string.IsNullOrEmpty(eventId)) {
                return new List<Session>();
            }

            return DocSession.Query<Session, SubmittedSessionsIndex>()
                             .Where(x =>
                                    x.Event.Id == eventId
                                    && x.User.Id == userId
                                    && x.Status != SessionStatus.Deleted
                );
        }

        public class SubmittedSessionsIndex : AbstractIndexCreationTask<Session> {
            public SubmittedSessionsIndex() {
                Map = sessions =>
                    from session in sessions
                    select new {
                        User_Id = session.User.Id,
                        Event_Id = session.Event.Id,
                        Status  = session.Status
                    };
            }
        }
    }
}