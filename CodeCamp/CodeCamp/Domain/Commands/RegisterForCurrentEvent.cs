using System;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;

namespace CodeCamp.Domain.Commands {
    public class RegisterForCurrentEvent : Command<Result<EventRegistration>> {
        protected override Result<EventRegistration> Execute() {
            throw new NotImplementedException();
        }
    }
}