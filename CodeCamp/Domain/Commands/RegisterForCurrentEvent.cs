using System;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;

namespace CodeCamp.Domain.Commands {
    public class RegisterForCurrentEvent : Command<Result> {
        protected override Result Execute() {
            switch(State.RegistrationStatus) {
                case RegistrationStatus.NoEventScheduled:
                    return Error("There is no event scheduled.");
                case RegistrationStatus.Registered:
                    return Success("You are already registered.");
                case RegistrationStatus.NotRegistered:
                    var registration = new Registration {
                        Id = Registration.IdFrom(
                            State.CurrentEventSlug(),
                            State.UserSlug()
                            ),
                        Event = new Reference {
                            Id = State.CurrentEvent.Id,
                            Name = State.CurrentEvent.Name
                        },
                        User = new Reference {
                            Id = State.User.Id,
                            Name = State.User.Username
                        },
                        RegisteredOn = Now()
                    };

                    DocSession.Store(registration);
                    State.RegistrationStatus = RegistrationStatus.Registered;

                    return Success("You have been successfully registered.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}