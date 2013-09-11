using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;

namespace CodeCamp.Domain.Commands {
    public class ChangeSessionStatus : Command<Result>.AdminOnly {
        readonly SessionStatus status;
        readonly string sessionId;

        public ChangeSessionStatus(string sessionId, SessionStatus status) {
            this.status = status;
            this.sessionId = sessionId;
        }

        protected override Result Execute() {
            var session = DocSession.Load<Session>(sessionId);
            if(session == null) {
                return NotFound();
            }

            session.Status = status;

            return SuccessFormat("The session \"{0}\" has been given a status of {1}.", session.Name, status);
        }
    }
}