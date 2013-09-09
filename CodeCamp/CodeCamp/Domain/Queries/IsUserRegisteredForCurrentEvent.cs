using System.Linq;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;
using Raven.Client.Indexes;

namespace CodeCamp.Domain.Queries {
    public class IsUserRegisteredForCurrentEvent : Query<RegistrationStatus> {
        protected override RegistrationStatus Execute() {
            if(State.CurrentEvent == null) {
                return RegistrationStatus.NoEventScheduled;
            }

            var registration = DocSession.Query<EventRegistration, RegistrationByUserAndEvent>()
                .SingleOrDefault(x =>
                    x.User.Id == State.User.Id
                    && x.Event.Id == State.CurrentEvent.Id
                );

            if(registration == null) {
                return RegistrationStatus.NotRegistered;
            }

            return RegistrationStatus.Registered;
        }

        public class RegistrationByUserAndEvent : AbstractIndexCreationTask<EventRegistration> {
            public RegistrationByUserAndEvent() {
                Map = eventRegistrations =>
                    from registration in eventRegistrations
                    select new {
                        User_Id = registration.User.Id,
                        Event_Id = registration.Event.Id
                    };
            }
        }
    }
}