using System.Linq;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;
using Raven.Client.Indexes;

namespace CodeCamp.Domain.Queries {
    public class GetUserRegistration : Query<EventRegistration> {
        readonly string eventId;
        readonly string userId;

        public GetUserRegistration(string eventId, string userId) {
            this.eventId = eventId;
            this.userId = userId;
        }

        protected override EventRegistration Execute() {
            return DocSession.Query<EventRegistration, RegistrationByUserAndEvent>()
                .SingleOrDefault(x =>
                    x.User.Id == userId
                    && x.Event.Id == eventId
                );
        }

        public class RegistrationByUserAndEvent : AbstractIndexCreationTask<EventRegistration> {
            public RegistrationByUserAndEvent() {
                Map = eventRegistrations =>
                    from registration in eventRegistrations
                    select new {
                        User_Id = registration.User.Id,
                        Event_Id = registration.Event.Id,
                        IsSpeaker = registration.IsSpeaker
                    };
            }
        }
    }
}