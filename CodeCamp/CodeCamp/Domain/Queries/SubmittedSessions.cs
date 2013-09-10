using System.Collections.Generic;
using System.Linq;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;
using Raven.Client.Indexes;

namespace CodeCamp.Domain.Queries {
    public class SubmittedSessions : Query<IEnumerable<Session>> {
        protected override IEnumerable<Session> Execute() {
            if(!State.UserIsLoggedIn()
               || State.RegistrationStatus != RegistrationStatus.Registered) {
                return new List<Session>();
            }

            return DocSession.Query<Session, SubmittedSessionsIndex>()
                .Where(x =>
                    x.Event.Id == State.CurrentEvent.Id
                    && x.Submitter.Id == State.User.Id
                );
        }

        public class SubmittedSessionsIndex : AbstractIndexCreationTask<Session> {
            public SubmittedSessionsIndex() {
                Map = sessions =>
                    from session in sessions
                    select new {
                        Submitter_Id = session.Submitter.Id,
                        Event_Id = session.Event.Id
                    };
            }
        }
    }
}