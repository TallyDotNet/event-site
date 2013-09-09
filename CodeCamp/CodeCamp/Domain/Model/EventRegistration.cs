using System;
using CodeCamp.Domain.Infrastructure;

namespace CodeCamp.Domain.Model {
    public class EventRegistration {
        public Reference User { get; set; }
        public Reference Event { get; set; }
        public DateTimeOffset RegisteredOn { get; set; }
    }
}